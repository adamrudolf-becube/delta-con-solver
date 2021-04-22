using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

// For linear algebra functions and classes
using MathNet.Numerics.LinearAlgebra.Single;

// For SQL IO
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace DeltaConSolver
{
    class users
    {
        // Initialization -------------------------------------------------------------------
        // SQL connection objects
        /*        SqlConnection con1;
                SqlConnectionStringBuilder csb1 = new SqlConnectionStringBuilder();
                SqlDataReader reader1;

                SqlConnection con2;
                SqlConnectionStringBuilder csb2 = new SqlConnectionStringBuilder();
                SqlDataReader reader2;
        */
        /*        SqlCommand command1 = new SqlCommand("select [user_a_id], [user_b_id] from [rudolf].[dbo].[mutual_mention_locations_table] ORDER BY user_a_id, user_b_id");
                SqlCommand command2 = new SqlCommand("select * from [rudolf].[dbo].[follow_mutual_graph] order by [user_a_id] ,[user_b_id]");
        */
        //File IO
        System.IO.StreamReader followFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\follower_graph_users_ordered.rpt");
        System.IO.StreamReader mentionFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\mention_graph_users_ordered.rpt");


        // Function expects 2 sorted int list and returns the number of the common elements
        public int overlap(List<int> a, List<int> b)
        {
            // Just to be sure
            //a.Sort();
            //b.Sort();
            int count = 0; // Counts the number of found verlaps
            int i = 0; // Iterates over a
            int j = 0; // Iterates over b
            int I = a.Count();
            int J = b.Count();

            // If two elements are equal, we count as one overlap and shift both lists.
            // If they are not, we increment the smaller until we find an equal element or
            // we "pass by" the one we are looking for i.e. there is no such element.
            while ((i < I) && (j < J))
            {
                if (a[i] == b[j])
                {
                    count++;
                    i++;
                    j++;
                }
                else
                    if (a[i] < b[j])
                        i++;
                    else
                        j++;
            }
            return count;
        }

        public int lineToInt(System.IO.StreamReader reader, string line)
        {
            string line_ = line; // Temporary container
            int user = -1; // The two int values. -1 is invalid: we seek for lines until we find valid pair
            if (line_ != null)
            {
                while (user == -1) // Do it until we find a valid pair
                {
                    try // If we can convert, do it
                    {
                        user = Convert.ToInt32(line_.ToString().Trim());
                    }
                    catch (Exception e) // If we can't, read teh next lne instead
                    {
                        if ((line_ = reader.ReadLine()) == null)
                        {
                            return -1;
                        }
                    }
                }
            }
            return user;
        }

        public void run()
        {
            // SQL initializations ---------------------------------------------------------
            /*            Console.WriteLine("Program started...");
                        csb1.DataSource = "retdb02";
                        csb1.InitialCatalog = "rudolf";
                        csb1.IntegratedSecurity = true;
                        csb1.ConnectTimeout = 100000;

                        con1 = new SqlConnection(csb1.ConnectionString);
                        con1.Open();
                        command1.Connection = con1;
                        command1.CommandTimeout = 100000;
            
                        csb2.DataSource = "retdb02";
                        csb2.InitialCatalog = "jszule";
                        csb2.IntegratedSecurity = true;
                        csb2.ConnectTimeout = 100000;

                        con2 = new SqlConnection(csb2.ConnectionString);
                        con2.Open();
                        command2.Connection = con2;
                        command2.CommandTimeout = 100000;
            */
            // One line of the file/table i.e. ne edge of the graph.
            int temp1 = 0;
            int temp2 = 0;
            List<int> nodes1 = new List<int>();
            List<int> nodes2 = new List<int>();

            int N1_all = 1554203;
            int N2_all = 3278469;

            // Reading the file row-by-row
            //EdgeComparer ec = new EdgeComparer();
            /*            reader1 = command1.ExecuteReader();
                        reader2 = command2.ExecuteReader();
            */
            string relat, which;
            string line1, line2;

            // Read the first line of both
            int V_1 = 1;
            int V_2 = 1;
            int V_1M2 = 0;

            Console.WriteLine("Started reading data and calculate vertex number and overlap...");
            bool Done1 = false;
            bool Done2 = false;


            // For diagnostics
            Stopwatch Stopper;
            Stopper = Stopwatch.StartNew();
            TimeSpan elapsed;
            TimeSpan left;
            while (!Done1 && !Done2) // While there is anything to read
            {
                //Console.WriteLine("While-ban. temp1 = "+temp1.ToString());
                // If the the rows are the same, we count this as an overlap
                if (temp1 == temp2)
                {
                    relat = "=";
                    which = "1++, 2++";
                    if ((line1 = mentionFile.ReadLine()) != null)
                        if ((line2 = followFile.ReadLine()) != null)
                        {
                            // Console.WriteLine
                            temp1 = lineToInt(mentionFile, line1);
                            temp2 = lineToInt(followFile, line2);
                        }
                        else
                            Done2 = true;
                    else
                        Done1 = true;

                    V_1M2++;
                    V_1++;
                    V_2++;
                }
                // If they are different, we read the next line only of the smaller until we find it's pair or a smaller element.
                // If we find a smaller element that means that has no par so we can move on
                else
                    if (temp1 < temp2)
                    {
                        relat = "<";
                        which = "1++";
                        if ((line1 = mentionFile.ReadLine()) != null)
                        {
                            temp1 = lineToInt(mentionFile, line1);
                            V_1++;
                        }
                        else
                            Done1 = true;
                    }
                    else
                    {
                        relat = ">";
                        which = "2++";
                        if ((line2 = followFile.ReadLine()) != null)
                        {
                            V_2++;
                            temp2 = lineToInt(followFile, line2);
                        }
                        else
                            Done2 = true;
                    }
                // Monitoring. If not needed, comment out.
                // Console.WriteLine("{0}\t{1} {2} E_1 = {3}, E_2 = {4}, E_1M2 = {4}", temp1.ToString(), relat, temp2.ToString(), E_1, E_2, E_1M2);

                if (V_2 % 100000 == 0)
                {
                    elapsed = Stopper.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
                        elapsed.Milliseconds / 10);

                    long ElapsedTicks = Stopper.ElapsedTicks;
                    long RemainingTicks = (long)(ElapsedTicks * ((1.0 * (N2_all - V_2)) / (1.0 * V_2)));
                    left = new TimeSpan(RemainingTicks);
                    string EstimatedLeft = String.Format("{0:00} days plus {1:00}:{2:00}:{3:00}.{4:00}",
                        left.Days, left.Hours, left.Minutes, left.Seconds,
                        left.Milliseconds / 10);

                    int percent = (int)((100.0 * V_2) / (1.0 * N2_all));
                    Console.WriteLine("{0} of {1} ({2}%) done, time elapsed: {3}, estimated time left: {4}", V_2, N2_all, percent, elapsedTime, EstimatedLeft);

                }
            }

            followFile.Close();
            mentionFile.Close();

            Console.WriteLine("E_1 = {0}, E_2 = {1}, E_1M2 = {2}", V_1, V_2, V_1M2);

            /*            Console.WriteLine("Calculate node number and overlap...");
                        var watch2 = Stopwatch.StartNew();
                        nodes1.Sort();
                        nodes2.Sort();
                        int V_1 = nodes1.Count();
                        int V_2 = nodes2.Count();
                        int V_1M2 = overlap(nodes1, nodes2);
                        var elapsedMs2 = watch2.ElapsedMilliseconds;
                        Console.WriteLine("Calculating finished in {0} seconds.", elapsedMs2 / 1000);

                        Console.WriteLine("Wrting output...");
                        double veo = 2.0 * (1.0 * V_1M2 + E_1M2) / (1.0 * V_1 + V_2 + E_1 + E_2); // The VEO similarity measure
                        double ged = V_1 + V_2 - 2.0 * V_1M2 + E_1 + E_2 - 2.0 * E_1M2; // The Graph Edit Distance similarity measure
        
                        Console.WriteLine("V_1 = {0}\tV_2 = {1}\tV_1M2 = {2}\nE_1 = {3}\tE_2 = {4}\tE_1M2 = {5}",V_1,V_2,V_1M2,E_1,E_2,E_1M2);
                        Console.WriteLine("Vertex/Edge overlap similarity measure (VEO) = {0}\nGraph Edit Distance similarity measure (GED) = {1}", veo, ged);
            */
        }
    }
}

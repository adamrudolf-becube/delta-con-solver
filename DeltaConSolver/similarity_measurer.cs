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
    // Class implements an algorithm to find the number of identical rows in two files containing edge list of graphs.
    class similarity_measurer
    {
        // Initialization -------------------------------------------------------------------
      
        //File IO
        System.IO.StreamReader followFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\follow_mutual_graph.txt");
        System.IO.StreamReader mentionFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\mention_mutual_graph.txt");


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

        // Function reads a line of text file and returns a Tuple of two Int numbers extracted from it.
        public Tuple<int, int> lineToTuple(System.IO.StreamReader reader, string line)
        {
            Tuple<int, int> result;
            Regex anySpaces = new Regex("[ ]+"); // One or more spaces as separator
            string line_ = line; // Temporary container
            string[] arrWords; // The container of split line
            int from = -1, to = -1; // The two int values. -1 is invalid: we seek for lines until we find valid pair
            if (line_ != null)
            {
                while ((from == -1) && (to == -1)) // Do it until we find a valid pair
                {
                    try // If we can convert, do it
                    {
                        arrWords = anySpaces.Split(line_);
                        from = Convert.ToInt32(arrWords[0].ToString().Trim());
                        to = Convert.ToInt32(arrWords[1].ToString().Trim());
                    }
                    catch (Exception e) // If we can't, read teh next lne instead
                    {
                        if ((line_ = reader.ReadLine()) == null)
                        {
                            return new Tuple<int, int>(-1, -1);
                        }
                    }
                }
            }
            result = new Tuple<int, int>(from, to);
            return result;
        }

        public void run()
        {
            // One line of the file/table i.e. one edge of the graph.
            Tuple<int, int> temp1 = new Tuple<int, int>(0,0);
            Tuple<int, int> temp2 = new Tuple<int, int>(0,0);
            List<int> nodes1 = new List<int>();
            List<int> nodes2 = new List<int>();

            int N1_all = 1640489; // from SQL
            int N2_all = 47120916;

            // Reading the table row-by-row
            EdgeComparer ec = new EdgeComparer();
            string relat, which;
            string[] arrWords1, arrWords2;
            string line1, line2;
            
            int from, to;

            // Read the first line of both
            int E_1 = 1;
            int E_2 = 1;
            int E_1M2 = 0;

            Console.WriteLine("Started reading data and calculate edge number and overlap...");
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
                if (ec.Compare(temp1, temp2) == 0)
                {
                    relat = "=";
                    which = "1++, 2++";
                    if ((line1 = mentionFile.ReadLine()) != null)
                        if ((line2 = followFile.ReadLine()) != null)
                        {
                            // Console.WriteLine
                            temp1 = lineToTuple(mentionFile, line1);
                            temp2 = lineToTuple(followFile, line2);
                        }
                        else
                            Done2 = true;
                    else
                        Done1 = true;
                    
                    E_1M2++;
                    E_1++;
                    E_2++;
                }
                // If they are different, we read the next line only of the smaller until we find it's pair or a smaller element.
                // If we find a smaller element that means that has no par so we can move on
                else
                    if (ec.Compare(temp1, temp2) < 0)
                    {
                        relat = "<";
                        which = "1++";
                        if ((line1 = mentionFile.ReadLine()) != null)
                        {
                            temp1 = lineToTuple(mentionFile, line1);
                            E_1++;
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
                            E_2++;
                            temp2 = lineToTuple(followFile, line2);
                        }
                        else
                            Done2 = true;
                    }
                // Monitoring. If not needed, comment it out.
                // Console.WriteLine("{0}\t{1} {2} E_1 = {3}, E_2 = {4}, E_1M2 = {4}", temp1.ToString(), relat, temp2.ToString(), E_1, E_2, E_1M2);

                if (E_2 % 100000 == 0)
                {
                    elapsed = Stopper.Elapsed;
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                        elapsed.Hours, elapsed.Minutes, elapsed.Seconds,
                        elapsed.Milliseconds / 10);

                    long ElapsedTicks = Stopper.ElapsedTicks;
                    long RemainingTicks = (long)(ElapsedTicks * ((1.0 * (N2_all - E_2)) / (1.0 * E_2)));
                    left = new TimeSpan(RemainingTicks);
                    string EstimatedLeft = String.Format("{0:00} days plus {1:00}:{2:00}:{3:00}.{4:00}",
                        left.Days, left.Hours, left.Minutes, left.Seconds,
                        left.Milliseconds / 10);

                    int percent = (int)((100.0 * E_2) / (1.0 * N2_all));
                    Console.WriteLine("{0} of {1} ({2}%) done, time elapsed: {3}, estimated tme left: {4}", E_2, N2_all, percent, elapsedTime, EstimatedLeft);
                    
                }
            }

            followFile.Close();
            mentionFile.Close();

            Console.WriteLine("E_1 = {0}, E_2 = {1}, E_1M2 = {2}", E_1, E_2, E_1M2);
        }
    }
}

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
    class dummy
    {
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
            System.IO.StreamReader testFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\test.txt");
            System.IO.StreamReader mentionFile = new System.IO.StreamReader(@"\\QSO\Public\rudolf\My Documents\mention_mutual_graph.txt");
            int counter = 0;
            string line;

            Regex anySpaces = new Regex("[ ]+");
            string[] arrWords1;
            Tuple<int, int> temp1;

            while ((line = testFile.ReadLine()) != null)
            {
                temp1 = lineToTuple(testFile, line);
                

                //temp1 = new Tuple<int, int>(Convert.ToInt32(arrWords1[0]), Convert.ToInt32(arrWords1[1]));
                //Convert.ToInt32(arrWords1[0]);

                Console.WriteLine("'{0}' '{1}'", temp1.Item1, temp1.Item2);
                counter++;
            }

            testFile.Close();
            mentionFile.Close();
            System.Console.WriteLine("There were {0} lines.", counter);
            // Suspend the screen.
            System.Console.ReadLine();  
        }
    }
}

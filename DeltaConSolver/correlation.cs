using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DeltaConSolver
{
    // Reads the 'degree_correlation.txt' file made by the correlation class of the mm_dist_dist02 assembly and calculates
    // the basic statistical properties of the dataset
    class correlation
    {
        List<int> a = new List<int>();
        List<int> b = new List<int>();
        int A = 0, B = 0;
        double c = 0;

        StreamReader inFile = new StreamReader(@"\\QSO\Public\rudolf\My Documents\degree_correlation.txt");
        string line;
        string[] arrWords = new string[3];
        public void run()
        {
            while ((line = inFile.ReadLine()) != null)
            {
                arrWords = line.Split(' ');
                a.Add(Convert.ToInt32(arrWords[1]));
                b.Add(Convert.ToInt32(arrWords[2]));
            }
            foreach (int i in a)
                A += i;
            foreach (int i in b)
                B += i;
            double[] f_a = new double[a.Count()];
            double[] f_b = new double[b.Count()];
            StreamWriter outFile = new StreamWriter(@"\\QSO\Public\rudolf\My Documents\nyeff.txt");

            for (int i = 0; i < a.Count(); i++)
            {
                f_a[i] = (double)a[i] / (double)A;
                f_b[i] = (double)b[i] / (double)B;
                outFile.WriteLine("{0} {1}", f_a[i], f_b[i]);
            }
            inFile.Close();
            outFile.Close();

            Console.WriteLine("avg a = {0}, avg b = {1}, sigma a = {2}, sigma b = {3}", Statistics.average(f_a), Statistics.average(f_b), Statistics.sigma(f_a), Statistics.sigma(f_b)); 
            c = Statistics.correlation(f_a, f_b);
            Console.WriteLine("C = {0}", c);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaConSolver
{
    // Basic statistical functions
    public static class Statistics
    {
        public static int sumOfSquares(int[] a)
        {
            int sum = 0;
            foreach(int i in a)
                sum += i*i;
            return sum;
        }

        public static double sumOfSquares(double[] a)
        {
            double sum = 0;
            foreach (double i in a)
                sum += i * i;
            return sum;
        }

        public static int sumXY(int[] a, int[] b)
        {
            int sum = 0;
            int n;
            if (a.Count() == b.Count())
                n = a.Count();
            else
            {
                Console.WriteLine("Error in simXY: not equal sized arrays.");
                return -2;
            }
            for (int i = 0; i < n; i++)
                sum += a[i] * b[i];
            return sum;
        }

        public static double sumXY(double[] a, double[] b)
        {
            double sum = 0;
            int n;
            if (a.Count() == b.Count())
                n = a.Count();
            else
            {
                Console.WriteLine("Error in simXY: not equal sized arrays.");
                return -2;
            }
            for (int i = 0; i < n; i++)
                sum += a[i] * b[i];
            return sum;
        }

        public static double average(int[] a)
        {
            double sum = 0;
            foreach (int i in a)
                sum += i;
            sum /= a.Count();
            return sum;
        }

        public static double average(double[] a)
        {
            double sum = 0;
            foreach (int i in a)
                sum += i;
            sum /= a.Count();
            return sum;
        }

        public static double sigma(int[] a)
        {
            double avg = average(a);
            int n = a.Count();
            double sum = 0;
            foreach (int i in a)
                sum += ((double)i - avg) * ((double)i - avg);
            return sum / (double)n;
        }

        public static double sigma(double[] a)
        {
            double avg = average(a);
            int n = a.Count();
            double sum = 0;
            foreach (double i in a)
                sum += (i - avg) * (i - avg);
            return sum / (double)n;
        }

        public static double correlation(int[] a, int[] b)
        {
            double avg_a = average(a);
            double avg_b = average(b);
            double sigma_a = sigma(a);
            double sigma_b = sigma(b);
            int n;
            if (a.Count() == b.Count())
                n = a.Count();
            else
            {
                Console.WriteLine("Error in correlation: not equal sized arrays.");
                return -2;
            }
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += (a[i] - avg_a) * (b[i] - avg_b);
            return sum / ((n - 1) * sigma_a * sigma_b);
        }

        public static double correlation(double[] a, double[] b)
        {
            double avg_a = average(a);
            double avg_b = average(b);
            double sigma_a = sigma(a);
            double sigma_b = sigma(b);
            int n;
            if (a.Count() == b.Count())
                n = a.Count();
            else
            {
                Console.WriteLine("Error in correlation: not equal sized arrays.");
                return -2;
            }
            double sum = 0;
            for (int i = 0; i < n; i++)
                sum += (a[i] - avg_a) * (b[i] - avg_b);
            return sum / ((n - 1) * sigma_a * sigma_b);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaConSolver
{
    // This class implements a vertex/edge overlap similarity measure calculator algorithm
    class VEO
    {
        // Function expects a list of <int, int, float> tuples as a sparse martix representation
        // and returns a list which contains every node exactly once and ordered
        public List<int> loadNodesToList(List<Tuple<int, int>> input)
        {
            List<int> result = new List<int>();
            foreach (Tuple<int, int> jihgz in input)
            {
                if (!result.Contains(jihgz.Item1))
                    result.Add(jihgz.Item1);
                if (!result.Contains(jihgz.Item2))
                    result.Add(jihgz.Item2);
            }
            result.Sort();
            return result;
        }

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

        public int TupleOverlap(List<Tuple<int, int>> a, List<Tuple<int, int>> b)
        {
            EdgeComparer ec = new EdgeComparer();
            // Just to be sure
            //a.Sort(ec);
            //b.Sort(ec);
            
            int count = 0; // Counts the number of found verlaps
            int i = 0; // Iterates over a
            int j = 0; // Iterates over b
            int I = a.Count();
            int J = b.Count();

            // If two elements are equal, we count as one overlap and shift both lists.
            // If they are not, we increment the smaller until we find an equal element or
            // we "pass by" the one we are looking for i.e. there is o such element.
            while ((i < I) && (j < J))
            {
                if (ec.Compare(a[i], b[j]) == 0)
                {
                    count++;
                    i++;
                    j++;
                }
                else
                    if (ec.Compare(a[i],b[j]) < 0)
                        i++;
                    else
                        j++;
            }
            return count;
        }

        // Fuction returns the Vertex/Edge Overlap similaritymeasure. sim_veo = (|E1 U E2| + |V1 U V2|) / (|E1| + |E2| + |V1| + |V2|)
        public void sim(List<Tuple<int, int>> a, List<Tuple<int, int>> b, out double veo, out double ged)
        {
            // Load all vertices to lists and sort them to compare
            List<int> users_a = new List<int>(loadNodesToList(a));
            List<int> users_b = new List<int>(loadNodesToList(b));
          
            // Counting
            int V_1 = users_a.Count(); // Cardinality of graph 'a' nodes
            int V_2 = users_b.Count(); // Cardinality of graph 'b' nodes
            int V_1M2 = overlap(users_a, users_b); // Cardinality of intersection of graph 'a' nodes and graph 'b' nodes

            int E_1 = a.Count(); // Cardinality of graph 'a' edges
            int E_2 = b.Count(); // Cardinality of graph 'b' edges
            int E_1M2 = TupleOverlap(a, b); // Cardinality of intersection of graph 'a' edges and graph 'b' edges
            Console.WriteLine(" V1 = {0}, V2 = {1}, V12 = {2}\nE1 = {3}, E2 = {4}, E12 = {5}", V_1, V_2, V_1M2, E_1, E_2, E_1M2);

            veo = 2.0 * (1.0 * V_1M2 + E_1M2) / (1.0 * V_1 + V_2 + E_1 + E_2); // The VEO similarity measure
            ged = V_1 + V_2 - 2.0 * V_1M2 + E_1 + E_2 - 2.0 * E_1M2; // The Graph Edit Distance similarity measure
        }
    }
}

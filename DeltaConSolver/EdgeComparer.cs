using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeltaConSolver
{
    // Compares two edge (two user pairs = two int pairs). They must be sorted by the first then the second user.
    class EdgeComparer : IComparer<Tuple<int, int>>
    {
        public int Compare(Tuple<int, int> x, Tuple<int, int> y)
        {
            if(x.Item1 == y.Item1)
                return x.Item2.CompareTo(y.Item2);
            else
                return x.Item1.CompareTo(y.Item1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// For linear algebra functions and classes
using MathNet.Numerics.LinearAlgebra.Single;

// For SQL IO
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace DeltaConSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            //similarity_measurer sm = new similarity_measurer();
            correlation sm = new correlation();
            //dummy sm = new dummy();
            sm.run();
        }
    }
}

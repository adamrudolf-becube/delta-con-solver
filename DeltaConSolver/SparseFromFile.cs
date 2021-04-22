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
    class SparseFromFile
    {
        // Initialization -------------------------------------------------------------------
        // SQL connection objects
        SqlConnection con;
        SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder();
        SqlCommand command;
        SqlDataReader reader;

        // The function reads an SQL database with rows of tuplets, and returns with the sparse matrix which is represented by the table
        public List<Tuple<int, int>> SQLtoLIST(SqlCommand command, string catalog)
        {
            // SQL initializations
            csb.DataSource = "retdb02";
            csb.InitialCatalog = catalog;
            csb.IntegratedSecurity = true;
            csb.ConnectTimeout = 100000;

            con = new SqlConnection(csb.ConnectionString);
            con.Open();
            command.Connection = con;
            command.CommandTimeout = 100000;
            
            // One line of the file/table i.e. ne edge of the graph.
            Tuple<int, int> temp;

            // List of the edges. Practically the definition of the graph.
            List<Tuple<int, int>> graph = new List<Tuple<int, int>>();

            // Reading the table row-by-row
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                temp = new Tuple<int, int>(Convert.ToInt32(reader[0].ToString().Trim()), Convert.ToInt32(reader[1].ToString().Trim()));
                graph.Add(temp);
            }

            // Create SparseMatrix based on the list graph
            return graph;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ProjetManhattan.Sources
{
    class AccesBDD : IAccesBDD
    {
        public string ConnectionString { get; set; }

        public AccesBDD(BaseConfig config)
        {
            ConnectionString = config.ConnectionString;
        }          

        public IDbConnection ConnexionBD()
        {
            try
            {
                SqlConnection connect = new SqlConnection(ConnectionString);
                connect.Open();
                return connect;
            }
            catch (SqlException sqlErreur)
            {
                Console.WriteLine("ERREUR CONNEXION SQL : " + sqlErreur.Message);
                throw;
            }
            //return null;
        }
    }
}

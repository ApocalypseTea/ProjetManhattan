using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using Microsoft.Data.SqlClient;

namespace ProjetManhattan.Sources
{
    class AccesBDD : IAccesBDD
    {
        public string ConnectionString { get; init; }

        public AccesBDD(BaseConfig config)
        {
            ConnectionString = config.connectionString;
        }          

        public SqlConnection ConnexionBD()
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

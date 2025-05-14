using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using ProjetManhattan.Sources;


namespace ProjetManhattan.Formatages
{
    public class AccesDBSQLite : IAccesBDD
    {
        private string _dbFileName;

        public string ConnectionString { get; set; }

        public AccesDBSQLite(string dbFileName = "resultatTraitement.db")
        {
            _dbFileName = dbFileName;
        }
        
        public IDbConnection ConnexionBD()
        {
            ConnectionString = "Data Source=" + _dbFileName;

            try
            {
                SqliteConnection connection = new SqliteConnection(ConnectionString);

                connection.Open();

                return connection;
            }
            catch (SqliteException _sqliteErreur)
            {
                Console.WriteLine("ERREUR CONNEXION SQL : " + _sqliteErreur.Message);
                throw;
            }
        }

        public SqliteConnection ConnectToTinyDBAndCreatingTableRecord()
        {
            string connectionString = "Data Source=" + _dbFileName;
            
            try {
                SqliteConnection connection = new SqliteConnection(connectionString);
            
                connection.Open();
                
                string creationTableRecord = "CREATE TABLE IF NOT EXISTS record (" +
                   "idRecord INTEGER PRIMARY KEY," +
                   "traitement TEXT NOT NULL,"+
                   "target TEXT NOT NULL," +
                   "date TEXT," +
                   "value TEXT," +
                   "propertyName TEXT," +
                   "description TEXT" +
                   ");" +
                   "CREATE INDEX IF NOT EXISTS IX_record ON record(" +
                   "idRecord," +
                   "traitement," +
                   "target," +
                   "date," +
                   "value," +
                   "propertyName," +
                   "description);";
                SqliteCommand commande = new SqliteCommand(creationTableRecord, connection);
                commande.ExecuteReader();

                return connection;
            }
            catch(SqliteException _sqliteErreur)
            {
                Console.WriteLine("ERREUR CONNEXION SQL : " + _sqliteErreur.Message);
                throw;
            }
        }

        
    }
}

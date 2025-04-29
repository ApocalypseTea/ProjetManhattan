using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace ProjetManhattan.Formatages
{
    public class AccesDBSQLite
    {
        private string _dbFileName;
        
        public AccesDBSQLite(string dbFileName = "resultatTraitement.db")
        {
            _dbFileName = dbFileName;
        }

        public SqliteConnection ConnectToDb()
        {
            string connectionString = "Data Source=" + _dbFileName;

            try
            {
                SqliteConnection connection = new SqliteConnection(connectionString);

                //connection.Open();

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
                //Console.WriteLine("Connecté à la BD SQLite");

                string creationTableRecord = "CREATE TABLE IF NOT EXISTS record (" +
                   "idRecord INTEGER PRIMARY KEY," +
                   "traitement TEXT NOT NULL,"+
                   "target TEXT NOT NULL," +
                   "date TEXT," +
                   "value TEXT," +
                   "propertyName TEXT," +
                   "description TEXT" +
                   ");";
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

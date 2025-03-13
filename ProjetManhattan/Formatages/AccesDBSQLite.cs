using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace ProjetManhattan.Formatages
{
    class AccesDBSQLite
    {
        private string _dbFileName;
        private string _dbPath;

        public AccesDBSQLite(string dbFileName = "resultTraitementDb.db")
        {
            _dbFileName = dbFileName;
        }

        public SqliteConnection ConnectToTinyDB()
        {
            string connectionString = "Data Source=" + _dbFileName;
            
            try {
                SqliteConnection connection = new SqliteConnection(connectionString);
            
                connection.Open();
                Console.WriteLine("connecté à la BD SQLite");
                /*
                //string creationTableTypeTraitement = "CREATE TABLE IF NOT EXISTS traitements_types (" +
                //    "idTraitement INTEGER PRIMARY KEY," +
                //    "nomTraitement TEXT NOT NULL);";

                //SqliteCommand commande = new SqliteCommand(creationTableTypeTraitement, connection);
                //commande.ExecuteReader();*/

                string creationTableRecord = "CREATE TABLE IF NOT EXISTS record (" +
                   "idRecord INTEGER PRIMARY KEY," +
                   "traitement TEXT NOT NULL,"+
                   "target TEXT NOT NULL," +
                   "date TEXT," +
                   "value REAL," +
                   "propertyName TEXT," +
                   "description TEXT" +
                   ");";
                SqliteCommand commande = new SqliteCommand(creationTableRecord, connection);
                commande.ExecuteReader();

                /*//string insertionTable = "INSERT INTO traitements_types (idTraitement, nomTraitement)" +
                //    "VALUES " +
                //    "(1, StatsIP)," +
                //    "(2, TempsRequete)," +
                //    "(3, AccesURL)," +
                //    "(4, BrisDeGlace)," +
                //    "(5, ValidationParInterne)," +
                //    "(6, ChangementIdentiteUtilisateur)," +
                //    "(7, ValidateurRCPAbsent);";

                //commande = new SqliteCommand(insertionTable, connection);*/

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

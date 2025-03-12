using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;


namespace ProjetManhattan
{
    class AccesDBSQLite
    {
        private string _dbFileName = "resultTraitementDb.db";
        private string _dbPath = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\SQLiteDB";

        public AccesDBSQLite(string DbFileName = "resultTraitementDb.db", string DbPath = @"C:\Users\AdeLas\source\repos\ProjetManhattan\ProjetManhattan\SQLiteDB")
        {
            this._dbFileName = DbFileName;
            this._dbPath = DbPath;
        }

        public SqliteConnection ConnectToTinyDB()
        {
            string pathAndFile = Path.Combine(_dbPath, _dbFileName);
            string connectionString = "Data Source=" + pathAndFile;
            Console.WriteLine(pathAndFile);

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("connecté a le BD SQLite");

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
                   //"FOREIGN KEY(traitement_ref)" +
                   //"REFERENCES traitements_types(idTraitement)" +
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
        }

        //public void AddRecordToDataBase(Record record)
        //{
        //    string requete = "INSERT INTO record (target, date, value, propertyName, description)" +
        //        $"VALUES ({record.Traitement}, {record.Target}, {record.Date}, {record.Value}, {record.PropertyName}, {record.Description});"
        //        ;
        //}

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using Unity;

namespace ProjetManhattan.Traitements
{
    public class TraitementTargetInfo :  BaseTraitementParRequeteSQL<LigneRequeteSQLiteTargetInfo>, ITraitement
    {
        public BaseConfig Config { get; set; }

        public string Name => "TargetInfo";
        public AccesDBSQLite AccesSQLiteDB { get; set; }

        private string _view;
        private string _target;
        private DateTime _dateDebut;
        private DateTime _dateFin;
        public string Query { get; set; }
        
        public TraitementTargetInfo(IUnityContainer container, BaseConfig config, string view, string target, DateTime dateDebut, string SQLiteDBName, DateTime dateFin = default) : base(container)
        {
            Config = config;
            _view = view;
            _target = target;
            _dateDebut = dateDebut;
            _dateFin = dateFin;
            AccesSQLiteDB = new AccesDBSQLite(SQLiteDBName);
        }

        public override void Execute()
        {
            Query = File.ReadAllText((string)Config._jConfig["views"][_view]["path"]);
            SqliteConnection connexion = AccesSQLiteDB.ConnectToDb();

            try
            {
                connexion.Open();
                IDbCommand commande = GetSQLCommand(connexion);
                IDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    LigneRequeteSQLiteTargetInfo line = ReadItem(reader);
                    _lines.Add(line);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"ERREUR CONNEXION SQLite : {ex.Message}");
            }
        }

        protected override LigneRequeteSQLiteTargetInfo ReadItem(IDataReader reader)
        {
            int colTarget = reader.GetOrdinal("target");
            int colJSON = reader.GetOrdinal("json");

            string target = reader.GetString(colTarget);
            string json = reader.GetString(colJSON);
            LigneRequeteSQLiteTargetInfo line = new LigneRequeteSQLiteTargetInfo(target,json);
            return line;
        }

        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            SqliteCommand commande = new SqliteCommand(Query, connection as SqliteConnection);
            commande.AddParameterWithValue("@Target", _target);
            commande.AddParameterWithValue("@DateDebut", _dateDebut);
            commande.AddParameterWithValue("@DateFin", _dateFin);
            return commande;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using ProjetManhattan.Traitements;
using Unity;

namespace ProjetManhattan.Analyses
{
    public class AnalyseTargetInfo
    {
        public BaseConfig Config { get; set; }

        public string Name => "TargetInfo";
        public IAccesBDD AccesSQLiteDB { get; set; }
        private string _view;
        public string View => _view;
        private string _target;
        public string Target => _target;
        private DateOnly _dateDebut;
        public DateOnly DateDebut => _dateDebut;
        private DateOnly _dateFin;
        public DateOnly DateFin => _dateFin;
        public string Query { get; set; }
        public List<LigneRequeteSQLiteTargetInfo> _lines;

        public AnalyseTargetInfo(BaseConfig config, string view, string target, DateTime dateDebut, string SQLiteDBName, DateTime dateFin = default)
        {
            Config = config;
            _view = view;
            _target = target;
            _dateDebut = DateOnly.FromDateTime(dateDebut);
            _dateFin = DateOnly.FromDateTime(dateFin);
            AccesSQLiteDB = new AccesDBSQLite(SQLiteDBName);
            Query = File.ReadAllText((string)config._jConfig["views"][view]["path"]);
            _lines = new List<LigneRequeteSQLiteTargetInfo>();
        }

        public void Execute()
        {
            using (IDbConnection connect = AccesSQLiteDB.ConnexionBD())
            {
                using (IDbCommand requete = GetSQLCommand(connect))
                {
                    using (IDataReader reader = requete.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LigneRequeteSQLiteTargetInfo item = ReadItem(reader);
                            _lines.Add(item);
                        }
                    }
                }
            }
        }

        protected LigneRequeteSQLiteTargetInfo ReadItem(IDataReader reader)
        {
            int colTarget = reader.GetOrdinal("target");
            int colJSON = reader.GetOrdinal("json");

            string target = reader.GetString(colTarget);
            string json = reader.GetString(colJSON);

            LigneRequeteSQLiteTargetInfo line = new LigneRequeteSQLiteTargetInfo(target,json);
            return line;
        }

        protected IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand commande = connection.CreateCommand();
            commande.CommandText = Query;
            commande.CommandType = CommandType.Text;

            commande.AddParameterWithValue("@Target", _target);
            commande.AddParameterWithValue("@DateDebut", _dateDebut);
            commande.AddParameterWithValue("@DateFin", _dateFin);
            return commande;
        }

        public string TargetInfoToJSON()
        {
            if (_lines.Count != 0)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                JObject jObject = new JObject();

                jObject.Add("target", _lines[0].Target);
                jObject.Add("json", JObject.Parse(_lines[0].Json));

                string json = JsonConvert.SerializeObject(jObject, settings);
                return json;
            } 
            else
            {
                return null;
            }
        }
    }
}
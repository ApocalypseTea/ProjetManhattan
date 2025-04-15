using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;

namespace ProjetManhattan.Formatages
{
    class SQLiteToZabbix
    {
        private const string QUERY_RANGE = "SELECT DISTINCT traitement, target, date, propertyName, description FROM record WHERE traitement=@traitement COLLATE NOCASE AND date BETWEEN @debutExport AND @finExport;";

        private const string QUERY_DATE = "SELECT DISTINCT traitement, target, date, propertyName, description FROM record WHERE traitement=@traitement COLLATE NOCASE AND DATE(date)=@dateExacte;";

        private List<ZabbixData> _zabbixListe;
        private SqliteConnection _connection;
        private DateTime _dateDebutExport;
        private DateTime _dateFinExport;
        private DateOnly _dateOnly;
        private bool isRange = false;
        private DateTime _dateValue;
        private string _descriptionValue;
        public SQLiteToZabbix(string _nomBD, DateTime dateDebutExport, DateTime dateFinExport)
        {
            _zabbixListe = new List<ZabbixData>();
            AccesDBSQLite accesDB = new AccesDBSQLite(_nomBD);
            _connection = accesDB.ConnectToTinyDB();
            _dateDebutExport = dateDebutExport;
            _dateFinExport = dateFinExport;
         
            isRange = true;
        }

        public SQLiteToZabbix(string _nomBD, DateOnly dateOnly)
        {
            _zabbixListe = new List<ZabbixData>();
            AccesDBSQLite accesDB = new AccesDBSQLite(_nomBD);
            _connection = accesDB.ConnectToTinyDB();

            _dateOnly= dateOnly;
        }

        public string GetJSONToZabbix(string nomTraitement, BaseConfig importConfig)
        {
            if (IsValidTraitement(nomTraitement, importConfig) == null)
            {
                Console.WriteLine("Traitement invalide");
                return null;
            }

            string finalQuery = "";
            if (isRange)
            {
                Console.WriteLine("Requete RANGE");
                finalQuery = QUERY_RANGE;
            }
            else
            {
                Console.WriteLine("Requete DATE");
                finalQuery = QUERY_DATE;
            }

                using (SqliteCommand requete = new SqliteCommand(finalQuery, _connection))
                {
                    if (isRange)
                    {
                        Console.WriteLine($"Parametres RANGE {nomTraitement} date debut {_dateDebutExport} date fin {_dateFinExport.AddDays(1)}");
                        requete.Parameters.AddWithValue("@traitement", nomTraitement);
                        requete.Parameters.AddWithValue("@debutExport", _dateDebutExport);
                        requete.Parameters.AddWithValue("@finExport", _dateFinExport.AddDays(1));
                    } 
                    else
                    {
                        Console.WriteLine($"Parametres DATE : {nomTraitement} et date {_dateOnly}");
                        Console.WriteLine($"La requete est {finalQuery}");
                        requete.Parameters.AddWithValue("@traitement", nomTraitement);
                        requete.Parameters.AddWithValue("@dateExacte", _dateOnly);
                    }

                    SqliteDataReader reader = requete.ExecuteReader();
                    while (reader.Read())
                    {
                        Console.WriteLine("Reader read");
                        int colTraitement = reader.GetOrdinal("traitement");
                        int colTarget = reader.GetOrdinal("target");
                        int colPropertyName = reader.GetOrdinal("propertyName");
                        int colDescription = reader.GetOrdinal("description");
                        int colDate = reader.GetOrdinal("date");

                        string traitement = reader.GetString(colTraitement);
                        string target = reader.GetString(colTarget);
                        string propertyName = reader.GetString(colPropertyName);
                        string description = reader.GetString(colDescription);
                        DateTime date = reader.GetDateTime(colDate);

                        ZabbixData zabbixObject = new ZabbixData(target, traitement, propertyName, description, date);
                        _zabbixListe.Add(zabbixObject);
                    }
                }

            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(_zabbixListe, settings);
        }
        public string GetValueFromTraitementTargetPropertyName (string nomTraitement, string nomTarget, string nomPropertyName, BaseConfig importConfig)
        {
            if (IsValidTraitement(nomTraitement, importConfig) == null)
            {
                return null;
            }


            string query = "SELECT value, date, description FROM record WHERE (traitement = @traitement COLLATE NOCASE OR propertyName = @propertyName COLLATE NOCASE) AND target = @target COLLATE NOCASE GROUP BY date HAVING date = MAX(date);";
            SqliteCommand requete = new SqliteCommand(query, _connection);

            requete.Parameters.AddWithValue("@traitement", nomTraitement);
            requete.Parameters.AddWithValue("@target", nomTarget);
            requete.Parameters.AddWithValue("@propertyName", nomPropertyName);

            SqliteDataReader reader = requete.ExecuteReader();
            string value = "no value";

            while (reader.Read())
            {
                int colVal = reader.GetOrdinal("value");
                int colDate = reader.GetOrdinal("date");
                int colDescription = reader.GetOrdinal ("description");

                value = reader.GetString(colVal);
                _dateValue = reader.GetDateTime(colDate);
                _descriptionValue = reader.GetString(colDescription);
            }

            Record recordValue = new Record();
            
            recordValue.Traitement = nomTraitement;
            recordValue.Target=nomTarget;
            recordValue.Date = _dateValue;
            recordValue.Value = value; 
            recordValue.PropertyName = nomPropertyName;
            recordValue.Description = _descriptionValue;

            string json = JsonConvert.SerializeObject(recordValue);
            return json;
        }

        public static string IsValidTraitement(string nomTraitement, BaseConfig importConfig)
        {
            GenerationNomTraitement generationNomTraitement = new GenerationNomTraitement(importConfig);

            bool isRealTreatment = false;

            foreach (var nomDeTraitementExistants in generationNomTraitement.AllTreatments.Keys)
            {
                if ((nomTraitement.ToLower()).Equals(nomDeTraitementExistants))
                {
                    isRealTreatment = true;
                }
            }

            if (isRealTreatment == false)
            {
                Console.WriteLine($"Erreur : le nom du traitement {nomTraitement} est incorrect.");
                Console.WriteLine("Traitements existants :");
                foreach (var nomDeTraitementExistants in generationNomTraitement.AllTreatments.Keys)
                {
                    Console.WriteLine($"- {nomDeTraitementExistants}");
                }
                return null;
            }

            else
            {
                return nomTraitement;
            }
        }



    }
}

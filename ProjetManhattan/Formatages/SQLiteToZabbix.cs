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
using ProjetManhattan.Traitements;

namespace ProjetManhattan.Formatages
{
    class SQLiteToZabbix
    {
        private const string QUERY = "SELECT DISTINCT traitement, target, _date, propertyName, description FROM _record WHERE traitement=@traitement AND _date BETWEEN @debutExport AND @finExport;";
        private List<ZabbixData> _zabbixListe;
        private SqliteConnection _connection;
        private DateTime _dateDebutExport;
        private DateTime _dateFinExport;
        public SQLiteToZabbix(string _nomBD, DateTime dateDebutExport, DateTime dateFinExport)
        {
            _zabbixListe = new List<ZabbixData>();
            AccesDBSQLite accesDB = new AccesDBSQLite(_nomBD);
            _connection = accesDB.ConnectToTinyDB();

            //Si erreur dans les dates, retourner les traitements concernant le jour meme
            if (dateDebutExport <= dateFinExport)
            {
                _dateDebutExport = dateDebutExport;
                _dateFinExport = dateFinExport;
            }
            else 
            {
                _dateDebutExport = DateTime.Now;
                _dateFinExport = DateTime.Now;
            }
        }
        public string GetJSONToZabbix(string nomTraitement)
        {
            using (SqliteCommand requete = new SqliteCommand(QUERY, _connection))
            {
                requete.Parameters.AddWithValue("@traitement", nomTraitement);
                requete.Parameters.AddWithValue("@debutExport", _dateDebutExport);
                requete.Parameters.AddWithValue("@finExport", _dateFinExport.AddDays(1));
                SqliteDataReader reader = requete.ExecuteReader();
                while (reader.Read())
                {
                    int colTraitement = reader.GetOrdinal("traitement");
                    int colTarget = reader.GetOrdinal("target");
                    int colPropertyName = reader.GetOrdinal("propertyName");
                    int colDescription = reader.GetOrdinal("description");
                    int colDate = reader.GetOrdinal("_date");

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
        public string GetValueFromTraitementTargetPropertyName (string nomTraitement, string nomTarget, string nomPropertyName)
        {
            string query = "SELECT value FROM _record WHERE traitement = @traitement AND target = @target AND propertyName = @propertyName GROUP BY _date HAVING _date = MAX(_date);";
            SqliteCommand requete = new SqliteCommand(query, _connection);

            requete.Parameters.AddWithValue("@traitement", nomTraitement);
            requete.Parameters.AddWithValue("@target", nomTarget);
            requete.Parameters.AddWithValue("@propertyName", nomPropertyName);

            SqliteDataReader reader = requete.ExecuteReader();
            string value = "no value";

            while (reader.Read())
            {
                int colVal = reader.GetOrdinal("value");
                value = reader.GetString(colVal);
            }
            return value;
        }
    }
}

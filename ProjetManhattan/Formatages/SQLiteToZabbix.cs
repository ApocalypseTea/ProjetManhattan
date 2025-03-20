using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Traitements;

namespace ProjetManhattan.Formatages
{
    class SQLiteToZabbix
    {
        private const string QUERY = "SELECT DISTINCT traitement, target, propertyName, description FROM record WHERE traitement=@traitement;";
        private List<ZabbixData> _zabbixListe;
        private SqliteConnection _connection;
        public SQLiteToZabbix(string _nomBD) {
            _zabbixListe = new List<ZabbixData>();
            AccesDBSQLite accesDB = new AccesDBSQLite(_nomBD);
            _connection = accesDB.ConnectToTinyDB();
        }
        public string GetJSONToZabbix(string nomTraitement)
        {
            using (SqliteCommand requete = new SqliteCommand(QUERY, _connection))
            {
                requete.Parameters.AddWithValue("@traitement", nomTraitement);
                SqliteDataReader reader = requete.ExecuteReader();
                while (reader.Read())
                {
                    int colTraitement = reader.GetOrdinal("traitement");
                    int colTarget = reader.GetOrdinal("target");
                    int colPropertyName = reader.GetOrdinal("propertyName");
                    int colDescription = reader.GetOrdinal("description");

                    string traitement = reader.GetString(colTraitement);
                    string target = reader.GetString(colTarget);
                    string propertyName = reader.GetString(colPropertyName);
                    string description = reader.GetString(colDescription);

                    ZabbixData zabbixObject = new ZabbixData(target, traitement, propertyName, description);
                    _zabbixListe.Add(zabbixObject);
                }
            }
            return JsonConvert.SerializeObject(_zabbixListe);
        }
        public float GetValueFromTraitementTargetPropertyName (string nomTraitement, string nomTarget, string nomPropertyName)
        {
            string query = "SELECT value FROM record WHERE traitement = @traitement AND target = @target AND propertyName = @propertyName GROUP BY date HAVING date = MAX(date);";
            SqliteCommand requete = new SqliteCommand(query, _connection);

            requete.Parameters.AddWithValue("@traitement", nomTraitement);
            requete.Parameters.AddWithValue("@target", nomTarget);
            requete.Parameters.AddWithValue("@propertyName", nomPropertyName);

            SqliteDataReader reader = requete.ExecuteReader();
            float value = -1;

            while (reader.Read())
            {
                int colVal = reader.GetOrdinal("value");
                value = reader.GetFloat(colVal);
            }
            return value;
        }
    }
}

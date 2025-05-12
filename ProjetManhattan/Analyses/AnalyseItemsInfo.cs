using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using ProjetManhattan.Traitements;
using ProjetManhattan.AnnexesTraitements;
using Azure.Core;

namespace ProjetManhattan.Analyses
{
    public class AnalyseItemsInfo
    {
        public BaseConfig Config { get; init; }
        public string Input { get; init; }
        public string Query { get; init; }

        public string Name => "ItemsInfo";

        public IAccesBDD AccesSQLiteDB { get; set; }

        public List<LigneRequeteSQLiteItemsInfo> _lines;

        public AnalyseItemsInfo(BaseConfig config, string input, string query) 
        {
            Config = config;
            Input = input;
            Query = query;
            AccesSQLiteDB = new AccesDBSQLite(input);
            _lines = new List<LigneRequeteSQLiteItemsInfo>();
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
                            LigneRequeteSQLiteItemsInfo item = ReadItem(reader);
                            _lines.Add(item);
                        }
                    }
                }
            }
        }

        protected LigneRequeteSQLiteItemsInfo ReadItem(IDataReader reader)
        {
            int colTarget = reader.GetOrdinal("item");
            string target = reader.GetString(colTarget);
            
            LigneRequeteSQLiteItemsInfo line = new LigneRequeteSQLiteItemsInfo(target);
            return line;
        }

        protected IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand commande = connection.CreateCommand();
            commande.CommandText = Query;
            commande.CommandType = CommandType.Text;
            return commande;
        }

        public string ItemsInfoToJSON()
        {
            if (_lines.Count != 0)
            {
                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                string json = JsonConvert.SerializeObject(_lines, settings);
                return json;
            }
            else
            {
                return null;
            }
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Configuration
{
    class BaseConfig
    {
        public string cheminFichierLog { get; init; }
        public string connectionString { get; init; }
        public string connectionStringIPLocator { get; init; }
        protected JObject jConfig;
        public DateTime dateTraitement { get; set; }
        public BaseConfig(string filename)
        {
            string json = File.ReadAllText(filename);
            jConfig = JObject.Parse(json);
            cheminFichierLog = (string)jConfig["sources"]["fichierDeLogIIS"]["cheminFichierLog"];
            connectionString = (string)jConfig["sources"]["accesBDD"]["connectionString"];
            connectionStringIPLocator = (string)jConfig["sources"]["accesBDD"]["connectionStringIPLocator"];
        }

        public T GetConfigTraitement<T>(string nomTraitement) where T : class
        {
            string nomTraitementEnCamelCase = transformToCamelCase(nomTraitement);


            return jConfig["traitements"][nomTraitementEnCamelCase].ToObject<T>();
        }

        private string transformToCamelCase(string nomTraitement)
        {
            char majusculeFirst = Char.ToLower(nomTraitement[0]);
            return majusculeFirst + nomTraitement.Substring(1);
        }
    }
}

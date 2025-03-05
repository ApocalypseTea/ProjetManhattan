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
        protected JObject jConfig;

        public BaseConfig(string filename)
        {
            string json = File.ReadAllText(filename);
            jConfig = JObject.Parse(json);
            cheminFichierLog = (string)jConfig["Sources"]["FichierDeLogIIS"]["CheminFichierLog"];
            connectionString = (string)jConfig["Sources"]["AccesBDD"]["connectionString"];
        }

        public T GetConfigTraitement<T>(string nomTraitement) where T : class
        {
            return jConfig["Traitements"][nomTraitement].ToObject<T>();
        }


        //public override string ToString()
        //{
        //    StringBuilder infosConfig = new StringBuilder();
        //    infosConfig.AppendLine($"Adresses IP Autorisées :");
        //    foreach (string ligne in adressesIPValides)
        //    {
        //        infosConfig.AppendLine(ligne);
        //    }
        //    infosConfig.AppendLine($"Seuil d'alerte du nombre de Requêtes Journalieres par IP : {seuilAlerteRequetesParIp}");
        //    infosConfig.AppendLine($"Chemin du Fichier Log Journalier : {cheminFichierLog}");

        //    return infosConfig.ToString();

        //}
    }
}

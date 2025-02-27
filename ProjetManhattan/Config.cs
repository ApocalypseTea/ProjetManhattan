using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class Config
    {
        public HashSet<string> adressesIPValides { get; init; }
        public int seuilAlerteRequetesParIp {  get; init; }
        public string cheminFichierLog {  get; init; }
        public int seuilAlerteTempsRequetes { get; init; }
        public HashSet<string> patternURLValide { get; init; }
        public Config(HashSet<string> adressesIPValides, int seuilAlerteRequetesParIp, string cheminFichierLog, int seuilAlerteTempsRequetes, HashSet<string> urlValides) 
        {
            this.adressesIPValides = adressesIPValides;
            this.seuilAlerteRequetesParIp = seuilAlerteRequetesParIp;
            this.cheminFichierLog = cheminFichierLog;
            this.seuilAlerteTempsRequetes = seuilAlerteTempsRequetes;
            this.patternURLValide = urlValides;
        }

        public override string ToString()
        {
            StringBuilder infosConfig = new StringBuilder();
            infosConfig.AppendLine($"Adresses IP Autorisées :");
            foreach (string ligne in adressesIPValides)
            {
                infosConfig.AppendLine(ligne);
            }
            infosConfig.AppendLine($"Seuil d'alerte du nombre de Requêtes Journalieres par IP : {seuilAlerteRequetesParIp}");
            infosConfig.AppendLine($"Chemin du Fichier Log Journalier : {cheminFichierLog}");

            return infosConfig.ToString();

        }
    }
}

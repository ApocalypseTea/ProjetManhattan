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
        public int seuilAlerteRequetesParIp;
        public Config(HashSet<string> adressesIPValides, int seuilAlerteRequetesParIp) 
        {
            this.adressesIPValides = adressesIPValides;
            this.seuilAlerteRequetesParIp = seuilAlerteRequetesParIp;
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
            return infosConfig.ToString();

        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    internal class IgnoreFastRequest : IFiltre
    {
        private int seuilTempsRequete;
        public IgnoreFastRequest(Config config) 
        {
            this.seuilTempsRequete = config.seuilAlerteTempsRequetes;
        }
        public bool Needed(LigneDeLog ligne)
        {
            return (seuilTempsRequete < ligne.timeTaken);
        }
    }
}

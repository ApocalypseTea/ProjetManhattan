using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Filtres
{
    internal class IgnoreFastRequest : IFiltre
    {
        private int seuilTempsRequete;
        public IgnoreFastRequest(int valeur) 
        {
            this.seuilTempsRequete = valeur;
        }
        public bool Needed(LigneDeLog ligne)
        {
            return (seuilTempsRequete < ligne.timeTaken);
        }
    }
}

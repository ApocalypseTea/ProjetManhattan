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
        private int _seuilTempsRequete;
        public IgnoreFastRequest(int valeur) 
        {
            this._seuilTempsRequete = valeur;
        }
        public bool Needed(LigneDeLog ligne)
        {
            return (_seuilTempsRequete < ligne.TimeTaken);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    class IgnoreURLWhiteList : IFiltre
    {
        private HashSet<string> _urlValides;

        public IgnoreURLWhiteList(Config config)
        {
            _urlValides = new HashSet<string>(config.patternURLValide);
        }
        public bool Needed(LigneDeLog ligne)
        {
            foreach(var url in _urlValides) 
            {
                if (Regex.IsMatch(ligne.csUriStem,url))
                {
                    return false;
                }
                
                //return !(ligne.csUriStem == url);
            }
            return true;
            
        }
    }
}

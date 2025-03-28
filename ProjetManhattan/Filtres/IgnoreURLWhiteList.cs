using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Filtres
{
    class IgnoreURLWhiteList : IFiltre
    {
        private HashSet<string> _urlValides;

        public IgnoreURLWhiteList(HashSet<string> regexs)
        {
            _urlValides = regexs;
        }
        public bool Needed(LigneDeLog ligne)
        {
            foreach(var url in _urlValides) 
            {
                if (Regex.IsMatch(ligne.CsUriStem,url))
                {
                    return false;
                }
                
                //return !(ligne.csUriStem == url);
            }
            return true;
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    internal class OnlyURLWhiteListAndHTTPError4xx : IFiltre
    {
        private HashSet<string> _urlValides;

        public OnlyURLWhiteListAndHTTPError4xx(HashSet<string> regexs)
        {
            _urlValides = regexs;
        }
        public bool Needed(LigneDeLog ligne)
        {
            if (ligne.CsStatutHTTP.ToString().StartsWith('4'))
            {
                foreach (var url in _urlValides)
                {

                    if (Regex.IsMatch(ligne.CsUriStem, url))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}

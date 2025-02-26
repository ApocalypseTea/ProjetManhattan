using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    internal class IgnoreWhiteList : IFiltre
    {
        private IPAutorisees _listeBlanche;

        public IgnoreWhiteList(Config config)
        {
            _listeBlanche = new IPAutorisees(config.adressesIPValides);
        }

        public bool Needed(LigneDeLog ligne)
        {
            string numIpClient = ligne.IpClient;
            HashSet<string> listeBlancheIp = _listeBlanche.adressesIPValides;

            return !listeBlancheIp.Contains(numIpClient);
        }
    }
}

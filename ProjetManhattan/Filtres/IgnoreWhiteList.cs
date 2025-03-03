using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Filtres
{
    internal class IgnoreWhiteList : IFiltre
    {
        private IPAutorisees _listeBlanche;

        public IgnoreWhiteList(HashSet<string> addressesIP)
        {
            _listeBlanche = new IPAutorisees(addressesIP);
        }

        public bool Needed(LigneDeLog ligne)
        {
            string numIpClient = ligne.IpClient;
            HashSet<string> listeBlancheIp = _listeBlanche.adressesIPValides;

            return !listeBlancheIp.Contains(numIpClient);
        }
    }
}

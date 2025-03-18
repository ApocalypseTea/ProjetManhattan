using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class IpClient
    {
        public string numeroIP { get; init; }
        public int nbConnexionJournaliere = 0;
        public IpClient(string numeroIP)
        {
            this.numeroIP = numeroIP;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class InfosURL
    {
        public string Url { get; init; }
        public IpClient AdresseIp { get; init; }
        public InfosURL (string url, IpClient adresseIp)
        {
            this.Url = url;
            this.AdresseIp = adresseIp;
        }
    }
}

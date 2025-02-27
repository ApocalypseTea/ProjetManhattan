using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class InfosURL
    {
        public string url { get; init; }
        public IpClient adresseIp { get; init; }
        public InfosURL (string url, IpClient adresseIp)
        {
            this.url = url;
            this.adresseIp = adresseIp;
        }



    }
}

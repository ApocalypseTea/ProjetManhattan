using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace ProjetManhattan
{
    internal class IpClient
    {
        public string adresseIP { get; init; }
        public int _nbConnexionJournaliere = 0;
        public string PaysOrigineIp { get; set; }
        public IpClient(string adresseIP)
        {
            this.adresseIP = adresseIP;
        }

        public long NumeroIp
        {
            get
            {
                long[] _conversionIpNumbers = { 16777216, 65536, 256, 1 };

                string[] numIp = adresseIP.Split('.');
                long numIpTotal = 0;
                for (int i = 0; i < 4; i++)
                {
                    numIpTotal += (int.Parse(numIp[i]) * _conversionIpNumbers[i]);
                }
                return numIpTotal;
            }
        }

        public override bool Equals(object? obj)
        {
            IpClient ipClient = obj as IpClient;
            if (ipClient != null)
            {
                return this.adresseIP.Equals(ipClient.adresseIP);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this.adresseIP.GetHashCode();
        }



    }
}

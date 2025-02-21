using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class LigneDeLog
    {
        private const int COL_DATE = 0;
        private const int COL_TIME = 1;
        private const int COL_IP_SOURCE = 2;
        private const int COL_METHODE_HTTP = 3;
        private const int COL_CS_URI_STEM = 4;
        private const int COL_URI_QUERY = 5;
        private const int COL_PORT = 6;
        private const int COL_USERNAME = 7;
        private const int COL_IP_CLIENT = 8;
        private const int COL_USER_AGENT_HTTP = 9;
        private const int COL_REFERER = 10;
        private const int COL_CS_STATUS_HTTP = 11;
        private const int COL_CS_SUBSTATUS = 12;
        private const int COL_CS_WIN32_STATUS = 13;
        private const int COL_TIME_TAKEN = 14;

        private DateTime _dateHeure;
        private string _ipSource;
        private VerbesHTTP _methodeHTTP;
        private string _csUriStem;
        private string _csUriQuery;
        private int _port;
        private string _username;
        public string IpClient { get; set; }
        private string _userAgentHTTP;
        private string _referer;
        private int _csStatutHTTP;
        private int _csSubStatut;
        private int _csWin32Status;
        private int _timeTaken;
        

        public LigneDeLog(string[] champsLog)
        {
            this._dateHeure = DateTime.Parse(champsLog[COL_DATE] + " " + champsLog[COL_TIME]);
            this._ipSource = champsLog[COL_IP_SOURCE];
            this._methodeHTTP = (VerbesHTTP)Enum.Parse(typeof(VerbesHTTP), champsLog[COL_METHODE_HTTP]);
            this._csUriStem = champsLog[COL_URI_QUERY];
            this._csUriQuery = champsLog[COL_URI_QUERY];
            this._port = int.Parse(champsLog[COL_PORT]);
            this._username = champsLog[COL_USERNAME];
            this.IpClient = champsLog[COL_IP_CLIENT];
            this._userAgentHTTP = champsLog[COL_USER_AGENT_HTTP];
            this._referer = champsLog[COL_REFERER];
            this._csStatutHTTP = int.Parse(champsLog[COL_CS_STATUS_HTTP]);
            this._csSubStatut = int.Parse(champsLog[COL_CS_SUBSTATUS]);
            this._csWin32Status = int.Parse(champsLog[COL_CS_WIN32_STATUS]);
            this._timeTaken = int.Parse(champsLog[COL_TIME_TAKEN]);
        }

        public override string ToString()
        {
            StringBuilder ligneDeLogTexte = new StringBuilder();
            ligneDeLogTexte.AppendLine($"Date et Heure: {_dateHeure}");
            ligneDeLogTexte.AppendLine($"Ip Serveur : {_ipSource}");
            ligneDeLogTexte.AppendLine($"Methode HTTP : {_methodeHTTP}");
            ligneDeLogTexte.AppendLine($"Port : {_port}");
            ligneDeLogTexte.AppendLine($"ip Client : {IpClient}");
            ligneDeLogTexte.AppendLine($"status HTTP : {_csStatutHTTP}");
            return ligneDeLogTexte.ToString();

        }

        public void AjouterIPClientAuDictionnaire(string numIpClient, Dictionary<string, IpClient> listingIPJournalieres, HashSet<string>listeBlancheIp)
        {
            foreach (var ip in listeBlancheIp)
            {
                if (numIpClient.Equals(ip))
                {
                    return;
                }
            }

            if (!listingIPJournalieres.ContainsKey(numIpClient))
            {
                IpClient nouvelleIp = new IpClient(numIpClient);
                listingIPJournalieres.Add(numIpClient, nouvelleIp);              
            }

            listingIPJournalieres[numIpClient].nbConnexionJournaliere++;

        }
    }
}

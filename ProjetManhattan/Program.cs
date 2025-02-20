using System;
using System.IO;

namespace ProjetManhattan
{        
    class LigneDeLog
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

        private DateTime dateHeure;
        private string ipSource;
        private VerbesHTTP methodeHTTP; 
        private string csUriStem;
        private string csUriQuery;
        private int port;
        private string username;
        private string ipClient;
        private string userAgentHTTP;
        private string referer;
        private int csStatutHTTP;
        private int csSubStatut;
        private int csWin32Status;
        private int timeTaken;

        public enum VerbesHTTP
        {
            GET,
            POST,
            PUT,
            DELETE,
            HEAD,
            TRACE,
            CONNECT,
            PATCH,
            OPTIONS
        }

        //Constructeur - prend un tableau de string en argument
        public LigneDeLog(string [] champsLog) 
        {
            this.dateHeure = DateTime.Parse(champsLog[COL_DATE] + " " + champsLog[COL_TIME]);
            this.ipSource = champsLog[COL_IP_SOURCE];
            this.methodeHTTP = (VerbesHTTP)Enum.Parse(typeof(VerbesHTTP), champsLog[COL_METHODE_HTTP]);
            this.csUriStem = champsLog[COL_URI_QUERY];
            this.csUriQuery = champsLog[COL_URI_QUERY];
            this.port = int.Parse(champsLog[COL_PORT]);
            this.username = champsLog[COL_USERNAME];
            this.ipClient = champsLog[COL_IP_CLIENT];
            this.userAgentHTTP = champsLog[COL_USER_AGENT_HTTP];
            this.referer = champsLog[COL_REFERER];
            this.csStatutHTTP = int.Parse(champsLog[COL_CS_STATUS_HTTP]);
            this.csSubStatut = int.Parse(champsLog[COL_CS_SUBSTATUS]);
            this.csWin32Status = int.Parse(champsLog[COL_CS_WIN32_STATUS]);
            this.timeTaken = int.Parse(champsLog[COL_TIME_TAKEN]);
        }

        public void AfficherLigneLog()
        {
            Console.WriteLine("Date et Heure: " + dateHeure);
            Console.WriteLine("Ip Serveur : " + ipSource);
            Console.WriteLine("Methode HTTP : " + methodeHTTP);
            Console.WriteLine("Port : " + port);
            Console.WriteLine("ip Client : " + ipClient);
            Console.WriteLine("status HTTP : " + csStatutHTTP);            
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            //Infos fichier log de travail
            string nomDeFichierLog = "ProjetManhattan\\test.txt";
            string cheminDeFichierLog = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string cheminFichierFinal = Path.Combine(cheminDeFichierLog,nomDeFichierLog);
            
            //Acceder au fichier ligne par ligne
            var lignes = File.ReadAllLines(cheminFichierFinal);
            foreach (string ligne in lignes)
            {
                if (ligne[0] == '#')
                {
                    continue;
                } 
                else
                {                    
                    string[] champs = ligne.Split(' ');
                    LigneDeLog ligneLog = new LigneDeLog(champs);
                    ligneLog.AfficherLigneLog();

                    Console.WriteLine();
                }
                
                
            }






        }
    }
}
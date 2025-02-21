using System;
using System.IO;

namespace ProjetManhattan
{
    class IpClient
    {
        //A passer en private des creation des accesseurs
        public string numeroIP;
        //A passer en private des creation des accesseurs
        public int nbConnexionJournaliere = 0;

        public IpClient(string numeroIP)
        {
            this.numeroIP = numeroIP;
        }
    }


    //class ListingJournalierIP
    //{
    //    List<IpClient> adressesIPJournaliere;
        

    //    //Constructeur
    //    public ListingJournalierIP()
    //    { 
    //        this.adressesIPJournaliere = new List<IpClient>();
    //    }
    //}

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

        private DateTime _dateHeure;
        private string _ipSource;
        private VerbesHTTP methodeHTTP; 
        private string _csUriStem;
        private string _csUriQuery;
        private int _port;
        private string _username;
        public string _ipClient { get; set; }
        //private string _ipClient2;
        //public string ipClient2 
        //{
        //    get { return _ipClient2; }
        //    private set { _ipClient2 = value; }
        //}

        //public string ipV4Client
        //{
        //    get { return _ipClient2; }
        //    //set { _ipClient2 = value; }
        //}
        private string _userAgentHTTP;
        private string _referer;
        private int _csStatutHTTP;
        private int _csSubStatut;
        private int _csWin32Status;
        private int _timeTaken;

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
            this._dateHeure = DateTime.Parse(champsLog[COL_DATE] + " " + champsLog[COL_TIME]);
            this._ipSource = champsLog[COL_IP_SOURCE];
            this.methodeHTTP = (VerbesHTTP)Enum.Parse(typeof(VerbesHTTP), champsLog[COL_METHODE_HTTP]);
            this._csUriStem = champsLog[COL_URI_QUERY];
            this._csUriQuery = champsLog[COL_URI_QUERY];
            this._port = int.Parse(champsLog[COL_PORT]);
            this._username = champsLog[COL_USERNAME];
            this._ipClient = champsLog[COL_IP_CLIENT];
            this._userAgentHTTP = champsLog[COL_USER_AGENT_HTTP];
            this._referer = champsLog[COL_REFERER];
            this._csStatutHTTP = int.Parse(champsLog[COL_CS_STATUS_HTTP]);
            this._csSubStatut = int.Parse(champsLog[COL_CS_SUBSTATUS]);
            this._csWin32Status = int.Parse(champsLog[COL_CS_WIN32_STATUS]);
            this._timeTaken = int.Parse(champsLog[COL_TIME_TAKEN]);
        }

        public void AfficherLigneLog()
        {
            Console.WriteLine("Date et Heure: " + _dateHeure);
            Console.WriteLine("Ip Serveur : " + _ipSource);
            Console.WriteLine("Methode HTTP : " + methodeHTTP);
            Console.WriteLine("Port : " + _port);
            Console.WriteLine("ip Client : " + _ipClient);
            Console.WriteLine("status HTTP : " + _csStatutHTTP);            
        }

               
        //Ajouter un element à la liste d'adresses IP
        public void AjouterIPClientAuListing(string numIpClient, List<IpClient> listingIPJournalieres)
        {
            bool isAlreadyCreated = false;            
            //Console.WriteLine("on veut ajouter " + numIpClient + " à la liste");
            //Verification d'une instance existante
            foreach (IpClient ip in listingIPJournalieres)
            {
                if (numIpClient == ip.numeroIP)
                {
                    //Console.WriteLine("L'adresse existe deja " + ip.nbConnexionJournaliere + " fois ");
                    ip.nbConnexionJournaliere++;
                    //Console.WriteLine("et une de plus");
                    isAlreadyCreated = true;

                    break;
                }
            }

            if (!isAlreadyCreated)
            {
                //sinon Creation d'une instance IPClient
                IpClient nouvelleIp = new IpClient(numIpClient);
                Console.WriteLine("on a créé une nouvelle instance a ajouter dans la liste");
                listingIPJournalieres.Add(nouvelleIp);
                //Console.WriteLine("ajout de l'ip " + nouvelleIp + " a la liste");
                nouvelleIp.nbConnexionJournaliere++;
            }
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

            //Créer une liste pour les informations IP
            List<IpClient> adressesIPJournaliere = new List<IpClient>();

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
                    //ligneLog.AfficherLigneLog();
                    ligneLog.AjouterIPClientAuListing(ligneLog._ipClient, adressesIPJournaliere);
                    //Console.WriteLine("on a ajouté : " + ligneLog.ipClient);
                }           
             }
            //Visualisation de la liste d'adresses IP Client
            foreach (var adresses in adressesIPJournaliere)
            {
                Console.WriteLine("La liste d'IP a enregistré : " + adresses.numeroIP + " qui s'est connectée " + adresses.nbConnexionJournaliere + " fois");
            }
            Console.WriteLine();

            //Tri des adresses IP par nombre de connexion
            var adressesIPJournaliereTriees = adressesIPJournaliere.OrderByDescending(adresse => adresse.nbConnexionJournaliere);
            //Visualisation de la liste d'adresses IP Client triées
            Console.WriteLine("Qui se connecte le plus ? ");
            foreach (var adresses in adressesIPJournaliereTriees)
            {

                Console.WriteLine("La liste d'IP a enregistré : " + adresses.numeroIP + " qui s'est connectée " + adresses.nbConnexionJournaliere + " fois");
            }






        }
    }
}
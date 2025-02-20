using System;
using System.IO;

namespace ProjetManhattan
{        
    class LigneDeLog
    {
        DateTime dateHeure;
        string ipSource;
        verbesHTTP methodeHTTP; 
        string csUriStem;
        string csUriQuery;
        int port;
        string username;
        string ipClient;
        string userAgentHTTP;
        string referer;
        string csStatutHTTP;
        string csSubStatut;
        string scWin32Status;
        int timeTaken;

        public enum verbesHTTP
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
            this.dateHeure = DateTime.Parse(champsLog[0] + " " + champsLog[1]);
            this.ipSource = champsLog[2];
            this.methodeHTTP = (verbesHTTP)Enum.Parse(typeof(verbesHTTP), champsLog[3]);
            this.csUriStem = champsLog[4];
            this.csUriQuery = champsLog[5];
            this.port = int.Parse(champsLog[6]);
            this.username = champsLog[7];
            this.ipClient = champsLog[8];
            this.userAgentHTTP = champsLog[9];
            this.referer = champsLog[10];
            this.csStatutHTTP = champsLog[11];
            this.csSubStatut = champsLog[12];
            this.scWin32Status = champsLog[13];
            this.timeTaken = int.Parse(champsLog[14]);
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

                    /*for (int i = 0; i < champs.Length; i++)
                    {
                        Console.WriteLine(champs[i]);
                        
                    }*/
                    Console.WriteLine();
                }
                
                
            }






        }
    }
}
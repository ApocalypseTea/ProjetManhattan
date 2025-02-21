using System;
using System.IO;

namespace ProjetManhattan
{    
    internal class Program
    {
        static void TrierAdressesIPParConnexion(Dictionary<string, IpClient> adressesIPJournaliere)
        {
            //Visualisation de la liste d'adresses IP Client du dictionnaire
            foreach (var adresses in adressesIPJournaliere)
            {
                Console.WriteLine("La liste d'IP du dictionnaire a enregistré : " + adresses.Key + " qui s'est connectée " + adresses.Value.nbConnexionJournaliere + " fois");
            }
            Console.WriteLine();

            //Tri des adresses IP par nombre de connexion
            List<IpClient> listingAdressesIP = adressesIPJournaliere.Values.ToList();
            var adressesIPJournaliereTriees = listingAdressesIP.OrderByDescending(adresse => adresse.nbConnexionJournaliere);

            //Visualisation de la liste d'adresses IP Client triées
            Console.WriteLine("Qui se connecte le plus ? ");
            foreach (var adresses in adressesIPJournaliereTriees)
            {
                Console.WriteLine("La liste d'IP a enregistré : " + adresses.numeroIP + " qui s'est connectée " + adresses.nbConnexionJournaliere + " fois");
            }
        }


        static void Main(string[] args)
        {
            //Liste Blanche des adresses IP autorisées
            HashSet<string> listeBlancheIP = new HashSet<string>();
            listeBlancheIP.Add("194.214.38.250");
            listeBlancheIP.Add("37.58.161.180");


            //Infos fichier log de travail
            string nomDeFichierLog = "ProjetManhattan\\test.txt";
            string cheminDeFichierLog = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string cheminFichierFinal = Path.Combine(cheminDeFichierLog, nomDeFichierLog);

            //Créer un dictionaire pour les informations IP
            Dictionary<string, IpClient> adressesIPJournaliere = new Dictionary<string, IpClient>();

            //Acceder au fichier ligne par ligne et ajout de chaque ligne à la collection d'adresses IP
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
                    
                    
                    ligneLog.AjouterIPClientAuDictionnaire(ligneLog.IpClient, adressesIPJournaliere, listeBlancheIP);
                    //Console.WriteLine("on a ajouté : " + ligneLog.ipClient);
                }
            }

            TrierAdressesIPParConnexion(adressesIPJournaliere);


            
        


    }

        









    }
    }

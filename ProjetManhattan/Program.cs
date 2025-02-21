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

        static HashSet<string> ImporterListeBlancheAdressesIP()
        {
            //Liste Blanche des adresses IP autorisées
            string nomDeFichierListeBlancheIP = "ProjetManhattan\\listeIPAutorisees.txt";
            string cheminDeFichiersRacine = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string cheminFichierFinalListeBlanche = Path.Combine(cheminDeFichiersRacine, nomDeFichierListeBlancheIP);
            
            HashSet<string> listeBlancheIP = new HashSet<string>();

            var ligneListeBlanche = File.ReadAllLines(cheminFichierFinalListeBlanche);
            foreach (var ligne in ligneListeBlanche)
            {
                listeBlancheIP.Add(ligne);
            }
            return listeBlancheIP;
        }

        static void Main(string[] args)
        {
            //Repertoire racine des documents
            string cheminDeFichiersRacine = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            

            //Infos fichier log de travail
            string nomDeFichierLog = "ProjetManhattan\\test.txt";            
            string cheminFichierFinalLogs = Path.Combine(cheminDeFichiersRacine, nomDeFichierLog);

            //Créer un dictionaire pour les informations IP
            Dictionary<string, IpClient> adressesIPJournaliere = new Dictionary<string, IpClient>();

            //Acceder au fichier ligne par ligne et ajout de chaque ligne à la collection d'adresses IP
            var lignes = File.ReadAllLines(cheminFichierFinalLogs);
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
                                        
                    ligneLog.AjouterIPClientAuDictionnaire(ligneLog.IpClient, adressesIPJournaliere, ImporterListeBlancheAdressesIP());
                }
            }

            TrierAdressesIPParConnexion(adressesIPJournaliere);
    }

        
    }
    }

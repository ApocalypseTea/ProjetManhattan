using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class RecuperateurIPDuLog
    {
        public string cheminDeFichier { get; init; }
        public string nomDeFichier { get; init; }
        public Dictionary<string, IpClient> adressesIPJournaliere { get; init; }
        public IPAutorisees listeBlanche { get; init; }

        public RecuperateurIPDuLog(string cheminDeFichier, string nomDeFichier, IPAutorisees listeBlanche)
        {
            this.cheminDeFichier = cheminDeFichier;
            this.nomDeFichier = nomDeFichier;
            this.listeBlanche = listeBlanche;

            string cheminFichierFinalLogs = Path.Combine(cheminDeFichier, nomDeFichier);

            //Créer un dictionaire pour les informations IP
            this.adressesIPJournaliere = new Dictionary<string, IpClient>();

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

                    ligneLog.AjouterIPClientAuDictionnaire(ligneLog.IpClient, adressesIPJournaliere, listeBlanche.adressesIPValides);
                }
            }
        }

        public void TrierAdressesIPParConnexion()
        {
            //Visualisation de la liste d'adresses IP Client du dictionnaire
            //foreach (var adresses in adressesIPJournaliere)
            //{
            //   Console.WriteLine("La liste d'IP du dictionnaire a enregistré : " + adresses.Key + " qui s'est connectée " + adresses.Value.nbConnexionJournaliere + " fois");
            //}
            //Console.WriteLine();

            //Tri des adresses IP par nombre de connexion
            List<IpClient> listingAdressesIP = adressesIPJournaliere.Values.ToList();
            var adressesIPJournaliereTriees = listingAdressesIP.OrderByDescending(adresse => adresse.nbConnexionJournaliere);

            //Visualisation de la liste d'adresses IP Client triées
            Console.WriteLine("Qui se connecte le plus ? ");
            foreach (var adresses in adressesIPJournaliereTriees)
            {
                Console.WriteLine(adresses.numeroIP + " s'est connectée " + adresses.nbConnexionJournaliere + " fois");
            }
        }




    }
}

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
        //public string nomDeFichier { get; init; }
        public Dictionary<string, IpClient> adressesIPJournaliere { get; init; }
        public IPAutorisees listeBlanche { get; init; }

        public RecuperateurIPDuLog(string cheminDeFichier, IPAutorisees listeBlanche)
        {
            this.cheminDeFichier = cheminDeFichier;       
            this.listeBlanche = listeBlanche;            

            //Créer un dictionaire pour les informations IP
            this.adressesIPJournaliere = new Dictionary<string, IpClient>();

            //Acceder au fichier ligne par ligne et ajout de chaque ligne à la collection d'adresses IP
            var lignes = File.ReadAllLines(cheminDeFichier);
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

        public void TrierAdressesIPParConnexion(int seuilAlerte)
        {
            //Tri des adresses IP par nombre de connexion
            List<IpClient> listingAdressesIP = adressesIPJournaliere.Values.ToList();
            List<IpClient> adressesIPJournaliereTriees = (listingAdressesIP.OrderByDescending(adresse => adresse.nbConnexionJournaliere)).ToList<IpClient>();            

            foreach(var adresse in adressesIPJournaliereTriees)
            {
                if (adresse.nbConnexionJournaliere > seuilAlerte)
                {
                    Console.WriteLine(adresse.numeroIP + " a effectué " + adresse.nbConnexionJournaliere + " requêtes au serveur aujourd'hui");
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var adresses in adressesIPJournaliere)
            {
               sb.AppendLine(adresses.Key + " s'est connectée " + adresses.Value.nbConnexionJournaliere + " fois");
            }
            
            return sb.ToString();
        }






    }
}

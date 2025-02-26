using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    internal class TraitementStatIP : ITraitement
    {
        private IFichierDeLog _source;
        private IFiltre _filtre;
        private IFormatage _sortie;
        private Dictionary<string, IpClient> _listingIPJournalieres;
        private int _seuilAlerte;

        public TraitementStatIP(Config config) 
        {
            _seuilAlerte = config.seuilAlerteRequetesParIp;
            _source = new FichierDeLogIIS(config);
            _filtre = new IgnoreWhiteList(config);
            _sortie = new OutputDisplay();
            _listingIPJournalieres = new Dictionary<string, IpClient>();
        }

        public void Display()
        {
            List<Notification> notifications = new List<Notification>();

            //Tri des adresses IP par nombre de connexion
            List<IpClient> listingAdressesIP = _listingIPJournalieres.Values.ToList();
            List<IpClient> adressesIPJournaliereTriees = (listingAdressesIP.OrderByDescending(adresse => adresse.nbConnexionJournaliere)).ToList<IpClient>();

            foreach (var adresse in adressesIPJournaliereTriees)
            {
                if (adresse.nbConnexionJournaliere > _seuilAlerte)
                {
                    Notification notification = new Notification((adresse.numeroIP + " a effectué " + adresse.nbConnexionJournaliere + " requêtes au serveur aujourd'hui"));
                    notifications.Add(notification);
                }
            }

            _sortie.Display(notifications);
        }

        public void Execute()
        {
            while(_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();

                if (ligne != null && _filtre.Needed(ligne))
                {
                    this.AddLine(ligne);
                }
                // filtre(ligne)
                // traitement(ligne)                
            }
        }

        private void AddLine(LigneDeLog line)
        {
            string numIpClient = line.IpClient;

            if (!_listingIPJournalieres.ContainsKey(numIpClient))
            {
                IpClient nouvelleIp = new IpClient(numIpClient);
                _listingIPJournalieres.Add(numIpClient, nouvelleIp);
            }

            _listingIPJournalieres[numIpClient].nbConnexionJournaliere++;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    internal class TraitementStatIP : BaseTraitement, ITraitement 
    {
        private Dictionary<string, IpClient> _listingIPJournalieres;
        private int _seuilAlerte;       

        public TraitementStatIP(BaseConfig config) : base(config)
        {
            ConfigStatsIP c = config.GetConfigTraitement<ConfigStatsIP>(nameof(TraitementStatIP));
            this.Filtre = new IgnoreWhiteList(c.adressesIPValides);

            _seuilAlerte = c.seuilAlerteRequetesParIp; 
            _listingIPJournalieres = new Dictionary<string, IpClient>();
        }

        public override void Display()
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

        
        protected override void AddLine(LigneDeLog line)
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

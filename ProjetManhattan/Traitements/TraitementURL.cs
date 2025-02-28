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
    internal class TraitementURL : Traitement, ITraitement
    {
        
        private List<InfosURL> _urlNonValides;

        public TraitementURL(Config config) : base (config, new IgnoreURLWhiteList(config))
        {          
            _urlNonValides = new List<InfosURL>();
        }
        public override void Display()
        {
            List<Notification> notificationsURLNonAutorisees = new List<Notification>();
            foreach (var requete in _urlNonValides)
            {
                Notification notification = new Notification($"L'adresse IP {requete.adresseIp.numeroIP} a cherché a acceder à :  {requete.url}");
                notificationsURLNonAutorisees.Add(notification);
            }
            _sortie.Display(notificationsURLNonAutorisees);
        }
        
        protected override void AddLine(LigneDeLog ligne)
        {
            IpClient ip = new IpClient(ligne.IpClient);
            InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
            _urlNonValides.Add(urlNonValide);
        }
    }
}

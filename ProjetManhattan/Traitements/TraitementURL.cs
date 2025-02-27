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
    class TraitementURL : ITraitement
    {
        private IFichierDeLog _source;
        private IFiltre _filtre;
        private IFormatage _sortie;
        private List<InfosURL> _urlNonValides;

        public TraitementURL(Config config)
        {
            _source = new FichierDeLogIIS(config);
            _filtre = new IgnoreURLWhiteList(config);
            _sortie = new OutputDisplay();
            _urlNonValides = new List<InfosURL>();
        }
        public void Display()
        {
            List<Notification> notificationsURLNonAutorisees = new List<Notification>();
            foreach (var requete in _urlNonValides)
            {
                Notification notification = new Notification($"L'adresse IP {requete.adresseIp.numeroIP} a cherché a acceder à :  {requete.url}");
                notificationsURLNonAutorisees.Add(notification);
            }
            _sortie.Display(notificationsURLNonAutorisees);
        }

        public void Execute()
        {
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne))
                {
                    this.AddLine(ligne);
                }
            }
        }

        private void AddLine(LigneDeLog ligne)
        {
            IpClient ip = new IpClient(ligne.IpClient);
            InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
            _urlNonValides.Add(urlNonValide);
        }
    }
}

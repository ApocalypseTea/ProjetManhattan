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
    internal class TraitementTempsRequete : Traitement, ITraitement
    {       
        private List<TempsRequete> _infosTempsRequetes;

        public TraitementTempsRequete(Config config) : base (config, new IgnoreFastRequest(config))
        {           
            _infosTempsRequetes = new List<TempsRequete>();           
        }

        public override void Display()
        {
            List<Notification> notificationsTempsRequete = new List<Notification>();
            foreach (var requete in _infosTempsRequetes)
            {
                Notification notification = new Notification($"La requete de l'IP {requete.ipClient.numeroIP} vers l'adresse {requete.url} a duré {requete.timeTaken} ms dont {requete.timeQuery} ms hors reseau");  
                notificationsTempsRequete.Add( notification );
            }
            _sortie.Display(notificationsTempsRequete);
        }
        protected override void AddLine(LigneDeLog ligne)
        {
            IpClient ip = new IpClient(ligne.IpClient);            
            TempsRequete tempsRequete = new TempsRequete(ip, ligne.timeTaken, ligne.csUriStem, ligne.NettoyageTempsRequeteHorsReseau(ligne.csUriQuery));

            _infosTempsRequetes.Add(tempsRequete);
        }
    }
}

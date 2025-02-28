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
    internal class TraitementTempsRequete : BaseTraitementParLigne<TempsRequete>, ITraitement
    {       
        public TraitementTempsRequete(Config config) : base (config, new IgnoreFastRequest(config))
        {           
        }   
        protected override Notification TranslateLigneToNotification(TempsRequete? requete)
        {
            Notification notification = new Notification($"La requete de l'IP {requete.ipClient.numeroIP} vers l'adresse {requete.url} a duré {requete.timeTaken} ms dont {requete.timeQuery} ms hors reseau");
            return notification;
        }

        protected override TempsRequete ParseLineIntoItem(LigneDeLog ligne, IpClient ip)
        {
            TempsRequete tempsRequete = new TempsRequete(ip, ligne.timeTaken, ligne.csUriStem, ligne.NettoyageTempsRequeteHorsReseau(ligne.csUriQuery));
            return tempsRequete;
        }
    }
}

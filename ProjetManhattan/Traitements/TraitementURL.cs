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
    internal class TraitementURL : BaseTraitementParLigne<InfosURL>, ITraitement
    {        
        public TraitementURL(Config config) : base (config, new IgnoreURLWhiteList(config))
        {          
        }
           
        //protected override void AddLine(LigneDeLog ligne)
        //{
        //    IpClient ip = new IpClient(ligne.IpClient);
        //    InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
        //    _items.Add(urlNonValide);
        //}

        protected override Notification TranslateLigneToNotification(InfosURL? requete)
        {
            Notification notification = new Notification($"L'adresse IP {requete.adresseIp.numeroIP} a cherché a acceder à :  {requete.url}");
            return notification;
        }

        protected override InfosURL ParseLineIntoItem(LigneDeLog ligne, IpClient ip)
        {
            InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
            return urlNonValide;
        }

    }
}

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
    // <InfosURL>
    internal class TraitementURL : BaseTraitementParLigne, ITraitement
    {        
        public TraitementURL(BaseConfig config) : base (config, new FichierDeLogIIS(config))
        {
            ConfigURLInvalides c = config.GetConfigTraitement<ConfigURLInvalides>(nameof(TraitementURL));
            this.Filtre = new IgnoreURLWhiteList(c.patternURLValide);
        }

        protected override void FillRecord(Record record, LigneDeLog ligne)
        {
            record.Traitement = nameof(TraitementURL);
            record.Target = ligne.csUriStem;
            record.PropertyName = "UrlDouteuse";
            record.Value = 1.0f;
        }

        //protected override void AddLine(LigneDeLog ligne)
        //{
        //    IpClient ip = new IpClient(ligne.IpClient);
        //    InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
        //    _items.Add(urlNonValide);
        //}

        //protected override Notification TranslateLigneToNotification(Record requete)
        //{
        //    //Notification notification = new Notification($"L'adresse IP {requete.adresseIp.numeroIP} a cherché a acceder à :  {requete.url}");
        //    Notification notification = new Notification($"{requete.Traitement} : {requete.Target} : {requete.Date} : {requete.PropertyName} : {requete.Value}");
        //    return notification;
        //}

        //protected override InfosURL ParseLineIntoItem(LigneDeLog ligne)
        //{
        //    IpClient ip = new IpClient(ligne.IpClient);
        //    InfosURL urlNonValide = new InfosURL(ligne.csUriStem, ip);
        //    return urlNonValide;
        //}

    }
}

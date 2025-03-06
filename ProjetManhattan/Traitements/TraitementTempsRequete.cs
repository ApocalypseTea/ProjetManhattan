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
    // <TempsRequete>
    internal class TraitementTempsRequete : BaseTraitementParLigne, ITraitement
    {       
        public TraitementTempsRequete(BaseConfig config) : base (config, new FichierDeLogIIS(config))
        {
            ConfigTempsRequetes c = config.GetConfigTraitement<ConfigTempsRequetes>(nameof(TraitementTempsRequete));
            this.Filtre = new IgnoreFastRequest(c.seuilAlerteTempsRequetes);
        }

        protected override void FillRecord(Record record, LigneDeLog ligne)
        {
            record.Traitement = nameof(TraitementTempsRequete);
            record.Target = ligne.csUriStem;
            record.PropertyName = "TimeTaken";
            record.Value = ligne.timeTaken;
        }

        //protected override Notification TranslateLigneToNotification(Record requete)
        //{
        //    //Notification notification = new Notification($"La requete de l'IP {requete.ipClient.numeroIP} vers l'adresse {requete.url} a duré {requete.timeTaken} ms dont {requete.timeQuery} ms hors reseau");
        //    Notification notification = new Notification($"{requete.Traitement} : {requete.Target} : {requete.Date} : {requete.PropertyName} : {requete.Value}");
        //    return notification;
        //}

        //protected override TempsRequete ParseLineIntoItem(LigneDeLog ligne)
        //{
        //    IpClient ip = new IpClient(ligne.IpClient);

        //    TempsRequete tempsRequete = new TempsRequete(ip, ligne.timeTaken, ligne.csUriStem, ligne.NettoyageTempsRequeteHorsReseau(ligne.csUriQuery));
        //    return tempsRequete;
        //}
    }
}

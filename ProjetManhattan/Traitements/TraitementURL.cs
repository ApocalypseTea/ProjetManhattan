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
    internal class TraitementURL : BaseTraitementParLigne, ITraitement
    {        
        public string Name
        {
            get
            {
                return "URL";
            }
        }
        public TraitementURL(BaseConfig config) : base (config, new FichierDeLogIIS(config))
        {
            ConfigURLInvalides c = config.GetConfigTraitement<ConfigURLInvalides>(nameof(TraitementURL));
            this.Filtre = new IgnoreURLWhiteList(c.PatternURLValide);
        }
        protected override void FillRecord(Record record, LigneDeLog ligne)
        {
            record.Traitement = "URL";
            record.Target = ligne.CsUriStem;
            record.PropertyName = "UrlDouteuse";
            record.Value = "true";
            record.Description = ligne.IpClient;
            record.Date = ligne.DateHeure;
        }
    }
}

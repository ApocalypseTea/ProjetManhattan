using Newtonsoft.Json.Linq;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementURL : BaseTraitementParLigne, ITraitement
    {        
        public string Name => "URL";
           
        public TraitementURL(BaseConfig config, IUnityContainer container) : base (container)
        {
        }
        protected override void FillRecord(Record[] records, LigneDeLog ligne)
        {
            JObject jsonDescription = new JObject();
            jsonDescription.Add("ip", ligne.IpClient);

            foreach (Record record in records)
            {
                record.Traitement = "URL";
                record.Target = ligne.CsUriStem;
                record.PropertyName = "UrlDouteuse";
                record.Value = "true";
                record.Description = jsonDescription.ToString();
                record.Date = ligne.DateHeure;
            }
        }

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigURLInvalides c = config.GetConfigTraitement<ConfigURLInvalides>(nameof(TraitementURL));
            this.Filtre = new IgnoreURLWhiteList(c.PatternURLValide);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementTempsRequete : BaseTraitementParLigne, ITraitement
    {       
        public string Name
        {
            get
            {
                return "TempsRequete";
            }
        }
        public TraitementTempsRequete(BaseConfig config, IUnityContainer container) : base (container)
        {
            ConfigTempsRequetes c = config.GetConfigTraitement<ConfigTempsRequetes>(nameof(TraitementTempsRequete));
            this.Filtre = new IgnoreFastRequest(c.SeuilAlerteTempsRequetes);
        }
        protected override void FillRecord(Record record, LigneDeLog ligne)
        {
            record.Traitement = "TempsRequete";
            record.Target = ligne.CsUriStem;
            record.PropertyName = "TimeTaken";
            record.Value = ligne.TimeTaken.ToString();
            record.Description = ligne.IpClient;
            record.Date = ligne.DateHeure;
        }
    }
}

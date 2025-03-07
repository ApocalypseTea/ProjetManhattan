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
    }
}

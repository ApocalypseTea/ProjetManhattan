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
    abstract class BaseTraitementParLigne : BaseTraitement<IFichierDeLog>
    {
        protected BaseTraitementParLigne(IUnityContainer container) : base(container)
        {
        }

        public override void Execute()
        {
            _source = this.Container.Resolve<IFichierDeLog>();
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne))
                {
                    Record[] record = ligne.ToRecords();
                    this.FillRecord(record, ligne);
                    this.AddLine(record);
                }
            }
        }
        protected abstract void FillRecord(Record[] record, LigneDeLog ligne);       
        protected virtual void AddLine(Record[] ligne)
        {
            this.AddRecord(ligne);
        }
    }
}

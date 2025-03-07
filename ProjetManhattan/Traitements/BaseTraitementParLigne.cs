using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    abstract class BaseTraitementParLigne : BaseTraitement<IFichierDeLog>
    {
        protected BaseTraitementParLigne(BaseConfig config, IFichierDeLog source) : base(config)
        {
            //_source = new FichierDeLogIIS(config);
            _source = source;
        }

        public override void Execute()
        {
            while (_source.HasLines())
            {
                LigneDeLog? ligne = _source.ReadLine();
                if (ligne != null && _filtre.Needed(ligne))
                {
                    Record record = ligne.ToRecord();
                    this.FillRecord(record, ligne);
                    this.AddLine(record);
                }
            }
        }

        protected abstract void FillRecord(Record record, LigneDeLog ligne);       

        //public override void Display()
        //{
        //    List<Notification> notifications = new List<Notification>();
        //    foreach (Record item in _items)
        //    {
        //        Notification notification = TranslateLigneToNotification(item);
        //        notifications.Add(notification);
        //    }
        //    _sortie.Display(notifications);
        //}


       

        protected virtual void AddLine(Record ligne)
        {
            //T tempsRequete = ParseLineIntoItem(ligne);

            this.AddItem(ligne);
            //throw new NotImplementedException();
        }
    }
}

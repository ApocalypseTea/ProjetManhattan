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
        protected List<Record> _items;
        protected BaseTraitementParLigne(BaseConfig config, IFichierDeLog source) : base(config)
        {
            _items = new List<Record>();
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

        public override void Display()
        {
            List<Notification> notifications = new List<Notification>();
            foreach (Record item in _items)
            {
                Notification notification = TranslateLigneToNotification(item);
                notifications.Add(notification);
            }
            _sortie.Display(notifications);
        }


        protected Notification TranslateLigneToNotification(Record requete)
        {
            Notification notification = new Notification($"{requete.Traitement} : {requete.Target} : {requete.Date} : {requete.PropertyName} : {requete.Value}");
            return notification;
        }

        protected virtual void AddLine(Record ligne)
        {
            //T tempsRequete = ParseLineIntoItem(ligne);

            _items.Add(ligne);
            //throw new NotImplementedException();
        }
    }
}

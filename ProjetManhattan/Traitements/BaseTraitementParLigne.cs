using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;

namespace ProjetManhattan.Traitements
{
    abstract class BaseTraitementParLigne<T> : BaseTraitement where T: class
    {
        protected List<T> _items;

        protected BaseTraitementParLigne(BaseConfig config) : base(config)
        {
            _items = new List<T>();
        }

        public override void Display()
        {
            List<Notification> notifications = new List<Notification>();
            foreach (T item in _items)
            {
                Notification notification = TranslateLigneToNotification(item);
                notifications.Add(notification);
            }
            _sortie.Display(notifications);
        }

        protected abstract Notification TranslateLigneToNotification(T? requete);

        protected override void AddLine(LigneDeLog ligne)
        {
            T tempsRequete = ParseLineIntoItem(ligne);

            _items.Add(tempsRequete);
        }

        protected abstract T ParseLineIntoItem(LigneDeLog ligne);
        
    }
}

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
    abstract class BaseTraitement<TSource> where TSource: ISource
    {
        private List<Record> _items;
        protected TSource _source;
        protected IFiltre _filtre;
        protected IFormatage _sortie;
        public BaseTraitement(BaseConfig config)
        {
            _sortie = new OutputDisplay();
            _items = new List<Record>();
        }
        public abstract void Execute();
        public virtual void Display()
        {
            List<Notification> notifications = new List<Notification>();
            foreach (Record item in _items)
            {
                Notification notification = TranslateLigneToNotification(item);
                notifications.Add(notification);
            }
            _sortie.AffichageNotifications(notifications);
        }
        public IFiltre Filtre
        {
            get { return _filtre; }
            set { _filtre = value; }
        }

        public void AddItem(Record item)
        {
            _items.Add(item);
        }

        protected Notification TranslateLigneToNotification(Record requete)
        {
            Notification notification = new Notification($"{requete.Traitement} : {requete.Target} : {requete.Date} : {requete.PropertyName} : {requete.Value}");
            return notification;
        }
    }
}

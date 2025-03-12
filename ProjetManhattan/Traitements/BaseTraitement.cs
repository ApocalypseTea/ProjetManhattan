using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Filtres;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Microsoft.Data.Sqlite;

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
            
            _sortie.AffichageRecord(_items);

            //Creation de la base de données de reception des records
            //AccesDBSQLite creationDBSQLite = new AccesDBSQLite();
            //SqliteConnection connection = creationDBSQLite.ConnectToTinyDB();

            ////Ajout des records dans la base de données créée
            //foreach (Record item in _items)
            //{
            //    RecordToSQLite ligneBD = new RecordToSQLite(item);
            //    ligneBD.AddRecordToDataBase(connection);
            //}
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
    }
}

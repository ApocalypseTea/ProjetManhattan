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
using System.Reflection;

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
        public virtual void Display(string exportDataMethod, string nomBD)
        {
            if (exportDataMethod.Equals("console"))
            {
                _sortie.AffichageRecord(_items);
            }

            if (exportDataMethod.Equals("bd"))
            {
                string userFileName = $"{nomBD}";
                AccesDBSQLite creationDBSQLite = new AccesDBSQLite(dbFileName: userFileName);

                SqliteConnection connection = creationDBSQLite.ConnectToTinyDB();

                foreach (Record item in _items)
                {
                    RecordToSQLite ligneBD = new RecordToSQLite(item);
                    ligneBD.AddRecordToDataBase(connection);
                }
            }
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

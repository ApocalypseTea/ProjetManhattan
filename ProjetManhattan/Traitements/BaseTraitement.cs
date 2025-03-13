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
        public virtual void Display(int exportDataMethod)
        {
            if (exportDataMethod == 0)
            {
                _sortie.AffichageRecord(_items);
            }
            if (exportDataMethod == 1)
            {
                AccesDBSQLite creationDBSQLite;
                string userFileName = null;
                Console.WriteLine("Nom du fichier BD souhaité : ");
                userFileName = Console.ReadLine();

                if (String.IsNullOrEmpty(userFileName) && !(userFileName is String))
                {
                    //Nom generique pour la BD resultTraitementDb.db
                    creationDBSQLite = new AccesDBSQLite();
                } 
                else
                {
                    userFileName += ".db";
                    creationDBSQLite = new AccesDBSQLite(dbFileName: userFileName);
                }
                    
                //Creation de la base de données de reception des records
                    
                SqliteConnection connection = creationDBSQLite.ConnectToTinyDB();

                //Ajout des records dans la base de données créée
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

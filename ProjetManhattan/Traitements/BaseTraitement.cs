﻿using System;
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
using Unity;

namespace ProjetManhattan.Traitements
{
    public abstract class BaseTraitement<TSource> where TSource: ISource
    {
        private List<Record[]> _records;
        protected TSource _source;
        protected IFiltre _filtre;
        protected IFormatage _sortie;
        protected IUnityContainer _container;
        public IUnityContainer Container => _container;
        
        public BaseTraitement(IUnityContainer container)
        {
            _container = container;
            _sortie = new OutputDisplay();
            _records = new List<Record[]>();
        }
        public abstract void Execute();
        public virtual void Display(string exportDataMethod, string nomBD)
        {
            if (exportDataMethod.Equals("console"))
            {
                foreach (Record[] record in _records)
                {
                    foreach (Record item in record)
                    {

                        _sortie.AffichageRecord(item);
                    }
                }
            }

            if (exportDataMethod.Equals("bd"))
            {
                string userFileName = $"{nomBD}";
                AccesDBSQLite creationDBSQLite = new AccesDBSQLite(dbFileName: userFileName);

                SqliteConnection connection = creationDBSQLite.ConnectToTinyDBAndCreatingTableRecord();

                foreach (Record[] item in _records)
                {
                    foreach (Record record in item)
                    {
                        RecordToSQLite ligneBD = new RecordToSQLite(record);
                        ligneBD.AddRecordToDataBase(connection);
                    }
                }
            }
        }

        public IFiltre Filtre
        {
            get { return _filtre; }
            set { _filtre = value; }
        }

        public void AddRecord(Record[] item)
        {
            _records.Add(item);
        }

        

    }
}

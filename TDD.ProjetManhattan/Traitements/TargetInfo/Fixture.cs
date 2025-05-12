using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using Unity;
using Unity.Events;
using ProjetManhattan.Sources;
using System.Data;
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Analyses;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class Fixture
    {
        public AnalyseTargetInfo? _traitement;
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        public IUnityContainer _container;
        public FakeAccesBDD _accesDBSQLite;
        private BaseConfig _fakeConfig;
        private string _fakeView;
        private string _fakeTarget;
        private DateTime _fakeDateDebut;
        private DateTime _fakeDateFin;
        private string _fakeSQLiteDBPath;
        public JObject _fakeJson;

        public Fixture() {
            _fakeConfig = new BaseConfig(FILENAME);
            _container = new UnityContainer();
            _accesDBSQLite = new FakeAccesBDD();
            _container.RegisterInstance<IUnityContainer>(_container);
            _container.RegisterInstance<IAccesBDD>(_accesDBSQLite);
        }

        public AnalyseTargetInfo WhenStartingTraitementTargetInfo()
        {
            _traitement = new AnalyseTargetInfo(this._fakeConfig, this._fakeView, this._fakeTarget, this._fakeDateDebut, this._fakeSQLiteDBPath, this._fakeDateFin);
            return _traitement;
        }

        public void GivingTraitementParameters(string fakeView, string fakeTarget, DateTime fakeDateDebut, string fakeSQLDbPath, DateTime fakeDateFin)
        {
            this._fakeView = fakeView;
            this._fakeTarget = fakeTarget;
            this._fakeDateDebut = fakeDateDebut;
            this._fakeDateFin = fakeDateFin;
            this._fakeSQLiteDBPath = fakeSQLDbPath;
        }

        public BaseConfig ThenConfigFile()
        {
            return _traitement.Config;        
        }

        public AnalyseTargetInfo ThenTraitementInstance()
        {
            return _traitement;
        }

        
        internal void WhenExecutingTraitement()
        {
            _traitement.Execute();
        }

        internal void GivingExistingData(string target, string json)
        {
            _traitement.AccesSQLiteDB = _accesDBSQLite;
            if (_accesDBSQLite.ExpectedData == null)
            {
                DataTable data = new DataTable("record");
                data.Columns.Add(new DataColumn("target", typeof(string)));
                data.Columns.Add(new DataColumn("json", typeof(string)));
                this._accesDBSQLite.ExpectedData = data;
            }

            DataRow row = _accesDBSQLite.ExpectedData.NewRow();
            row["target"] = target;
            row["json"] = json;
            _accesDBSQLite.ExpectedData.Rows.Add(row);
        }

        public IDbCommand ThenCommand()
        {
            return this._accesDBSQLite.Connection.CreateCommand();
        }

        public void WhenTryingToConnectToDatabase()
        {
            _traitement.AccesSQLiteDB.ConnectionString = $"Data Source={this._fakeSQLiteDBPath}";
        }

        public JObject ThenJSONResult()
        {
            //    this._fakeJson = JObject.Parse(_traitement.TargetInfoToJSON());
            //    return this._fakeJson;
            JObject jObject = JObject.Parse(_traitement.TargetInfoToJSON());
            return jObject;

        }

        public void WhenExecutingTargetInfoToJSON()
        {
            _traitement.TargetInfoToJSON();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Analyses;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using TDD.ProjetManhattan.Traitements.ItemsInfo;

namespace TDD.ProjetManhattan.Traitements.Items
{
    internal class Fixture
    {
        protected AnalyseItemsInfo _traitement;
        protected string _configFileName = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";
        protected string _nomBaseDonneesInput;
        protected string _query;
        public BaseConfig _config;
        private FakeAccesBDD _accesSQLite;

        public Fixture() 
        {
            _config = new BaseConfig(_configFileName);
            _accesSQLite = new FakeAccesBDD();

        }

        internal void GivenConfigFilename(string filename)
        {
            this._configFileName = filename;
            _config=new BaseConfig(_configFileName);
        }

        internal void GivenInstanceParameters(string baseDeDonnees, string query)
        {
            _nomBaseDonneesInput = baseDeDonnees;
            _query = query;
        }

        internal AnalyseItemsInfo ThenAnalyseInstance()
        {
            return this._traitement;
        }

        internal void WhenCreatingAnalyse()
        {
            string query = File.ReadAllText((string)_config._jConfig["items"][_query]["path"]);
            _traitement = new AnalyseItemsInfo(_config, _nomBaseDonneesInput, query);
        }

        internal void GivenDatabaseResult(string itemResult)
        {
            if (_accesSQLite.ExpectedData == null)
            {
                DataTable data = new DataTable("record");
                data.Columns.Add(new DataColumn("item", typeof(string)));
                this._accesSQLite.ExpectedData = data;
            }

            DataRow row = _accesSQLite.ExpectedData.NewRow();
            row["item"] = itemResult;
           
            _accesSQLite.ExpectedData.Rows.Add(row);
        }

        internal void WhenExecutingAnalyse()
        {
            this.WhenCreatingAnalyse();
            this._traitement.AccesSQLiteDB = _accesSQLite;
            this._traitement.Execute();
        }
    }
}

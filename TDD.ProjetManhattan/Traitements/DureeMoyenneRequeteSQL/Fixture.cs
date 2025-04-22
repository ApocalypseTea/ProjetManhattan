using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProjetManhattan;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using ProjetManhattan.Traitements;
using Unity;

namespace TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL
{
    internal class Fixture
    {
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        private TraitementDureeMoyenneRequeteSQL _traitement;
        private IUnityContainer _container;
        private FakeAccesBDD _fakeAccesBDD;

        public Fixture()
        {
            _container = new UnityContainer();
            _fakeAccesBDD = new FakeAccesBDD();

            _container.RegisterInstance<IUnityContainer>(_container);
            _container.RegisterInstance<IAccesBDD>(_fakeAccesBDD);
        }

        private static BaseConfig GetConfig()
        {
            return new BaseConfig(FILENAME);
        }

        internal void GivenExistingRow(long duree, string storedProcedureName)
        {
            AddRow(duree, storedProcedureName);
        }

        internal void WhenCreatingTraitement()
        {
            BaseConfig config = GetConfig();
            _traitement = new TraitementDureeMoyenneRequeteSQL(_container);
        }

        internal void WhenExecutingTraitement()
        {
            WhenCreatingTraitement();
            _traitement.Execute();
        }


        internal TraitementDureeMoyenneRequeteSQL ThenTraitement()
        {
            return _traitement;
        }

        internal IReadOnlyList<LigneRequeteDureeMoyenneSQL> ThenResults()
        {
            return _traitement.Items;
        }


        private void AddRow(params object[] row)
        {
            if (_fakeAccesBDD.ExpectData == null)
            {
                DataTable data = new DataTable();
                data.Columns.Add(new DataColumn("avg_elapsed_time", typeof(long)));
                data.Columns.Add(new DataColumn("stored_procedure", typeof(string)));
                this._fakeAccesBDD.ExpectData = data;
            }

            _fakeAccesBDD.ExpectData.Rows.Add(row);
        }

        internal void ThenShouldConnectToDatabase()
        {
            _fakeAccesBDD.HasCreatedInstance().Should().BeTrue();
        }

        internal void ThenShouldInterrogateSpecificTable(string tableName)
        {
            _fakeAccesBDD.CreatedSqlCommand().CommandText.Should().Contain($"FROM {tableName}");
        }
    }
}

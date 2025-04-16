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
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;
using Unity;

namespace TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQLTests
{
    [TestClass]
    public class DureeMoyenneRequeteSQLTests
    {
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        private IUnityContainer _container;
        private FakeAccesBDD _fakeAccesBDD;

        [TestInitialize]
        public void Init()
        {
            _fakeAccesBDD = new FakeAccesBDD();
            _container = new UnityContainer();
            _container.RegisterInstance<IUnityContainer>(_container);
            _container.RegisterInstance<IAccesBDD>(_fakeAccesBDD);
        }

        [TestMethod]
        public void CanCreateInstance()
        {
            BaseConfig config = GetConfig();
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(_container);

            Assert.IsNotNull(traitement);
            traitement.Should().NotBeNull();
        }

        private static BaseConfig GetConfig()
        {
            return new BaseConfig(FILENAME);
        }

        [TestMethod]
        public void ShouldHaveAName()
        {
            BaseConfig config = GetConfig();
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            Assert.AreEqual("DureeTraitementRequeteSQL", traitement.Name);
            traitement.Name.Should().Be("DureeTraitementRequeteSQL");
        }

        [TestMethod]
        public void ShouldExecute()
        {
            BaseConfig config = GetConfig();
            ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            traitement.Execute();
            Assert.IsTrue(true);
            true.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldExecuteAndReturnOneResult()
        {
            BaseConfig config = GetConfig();
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            AddRow(1, "TOTO");
            traitement.Execute();
            Assert.IsTrue(traitement.Items.Count == 1);
            traitement.Items.Count.Should().Be(1);
        }

        [TestMethod]
        public void ShouldExecuteAndReturnTwoResults()
        {
            BaseConfig config = GetConfig();
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);

            AddRow(10, "RIRI");
            AddRow(123, "FIFI");
            
            
            //AddRow(123, "FIFI");

            traitement.Execute();
            Assert.IsTrue(traitement.Items.Count == 2);
            traitement.Items.Count.Should().Be(2);
        }

        [TestMethod]
        public void ExecuteShouldConnectToDatabase()
        {
            BaseConfig config = GetConfig();
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            traitement.Execute();

            Assert.IsTrue(_fakeAccesBDD.HasCreatedInstance());
            _fakeAccesBDD.HasCreatedInstance().Should().BeTrue();
        }

        [TestMethod]
        public void ExecuteShouldInterrogateSpecificTable()
        {
            BaseConfig config = GetConfig();
            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            traitement.Execute();

            Assert.Contains("FROM sys.dm_exec_procedure_stats", _fakeAccesBDD.CreatedSqlCommand().CommandText);
            _fakeAccesBDD.CreatedSqlCommand().CommandText.Should().Contain("FROM sys.dm_exec_procedure_stats");
        }

        [TestMethod]
        public void ResultShouldHaveDuree()
        {
            BaseConfig config = GetConfig();
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("avg_elapsed_time", typeof(float)));
            data.Columns.Add(new DataColumn("stored_procedure", typeof(string)));
            data.Rows.Add(42, "riri");
            this._fakeAccesBDD.ExpectData = data;

            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            traitement.Execute();
            Assert.AreEqual(42, traitement.Items[0].Duree);
            traitement.Items[0].Duree.Should().Be(42);
        }

        [TestMethod]
        public void ResultSHouldHaveStoredProcedureName()
        {
            BaseConfig config = GetConfig();
            //DataTable data = new DataTable();
            //data.Columns.Add(new DataColumn("avg_elapsed_time", typeof(float)));
            //data.Columns.Add(new DataColumn("stored_procedure", typeof(string)));
            //data.Rows.Add(42, "riri");
            //this._fakeAccesBDD.ExpectData = data;
            AddRow(42, "riri");

            TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            traitement.Execute();
            //Assert.AreEqual("riri", traitement.Items[0].StoredProcedure);
            traitement.Items[0].StoredProcedure.Should().Be("riri");
        }

        private DureeMoyenneRequeteSQLTests AddRow(params object[] row)
        {
            if (_fakeAccesBDD.ExpectData == null)
            {
                DataTable data = new DataTable();
                data.Columns.Add(new DataColumn("avg_elapsed_time", typeof(float)));
                data.Columns.Add(new DataColumn("stored_procedure", typeof(string)));
                this._fakeAccesBDD.ExpectData = data;
            }

            _fakeAccesBDD.ExpectData.Rows.Add(row);

            return this;
        }

    }
}

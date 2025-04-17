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
        private Fixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
        }

        [TestMethod]
        public void CanCreateInstance()
        {
            //BaseConfig config = GetConfig();
            //ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(_container);

            //Assert.IsNotNull(traitement);
            //traitement.Should().NotBeNull();

            _fixture.WhenCreatingTraitement();
            _fixture.ThenTraitement().Should().NotBeNull();
        }     

        [TestMethod]
        public void ShouldHaveAName()
        {
            _fixture.WhenCreatingTraitement();
            _fixture.ThenTraitement().Name.Should().Be("DureeTraitementRequeteSQL");

            //BaseConfig config = GetConfig();
            //ITraitement traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //Assert.AreEqual("DureeTraitementRequeteSQL", traitement.Name);
            //traitement.Name.Should().Be("DureeTraitementRequeteSQL");
        }      

        [TestMethod]
        public void ShouldExecuteAndReturnOneResult()
        {
            _fixture.GivenExistingRow(1, "TOTO");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenResults().Count.Should().Be(1);

            //BaseConfig config = GetConfig();
            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //AddRow(1, "TOTO");
            //traitement.Execute();
            //Assert.IsTrue(traitement.Items.Count == 1);
            //traitement.Items.Count.Should().Be(1);
        }

        [TestMethod]
        public void ShouldExecuteAndReturnTwoResults()
        {
            _fixture.GivenExistingRow(10, "RIRI");
            _fixture.GivenExistingRow(123, "FIFI");

            _fixture.WhenExecutingTraitement();
            _fixture.ThenResults().Count.Should().Be(2);

            //BaseConfig config = GetConfig();
            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);

            //AddRow(10, "RIRI");
            //AddRow(123, "FIFI");


            ////AddRow(123, "FIFI");

            //traitement.Execute();
            //Assert.IsTrue(traitement.Items.Count == 2);
            //traitement.Items.Count.Should().Be(2);
        }

        [TestMethod]
        public void ExecuteShouldConnectToDatabase()
        {
            _fixture.WhenExecutingTraitement();
            _fixture.ThenShouldConnectToDatabase();

            //BaseConfig config = GetConfig();
            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //traitement.Execute();

            //Assert.IsTrue(_fakeAccesBDD.HasCreatedInstance());
            //_fakeAccesBDD.HasCreatedInstance().Should().BeTrue();
        }

        [TestMethod]
        public void ExecuteShouldInterrogateSpecificTable()
        {
            _fixture.WhenExecutingTraitement();
            _fixture.ThenShouldInterrogateSpecificTable("sys.dm_exec_procedure_stats");
            //BaseConfig config = GetConfig();
            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //traitement.Execute();

            //Assert.Contains("FROM sys.dm_exec_procedure_stats", _fakeAccesBDD.CreatedSqlCommand().CommandText);
            //_fakeAccesBDD.CreatedSqlCommand().CommandText.Should().Contain("FROM sys.dm_exec_procedure_stats");
        }

        [TestMethod]
        public void ResultShouldHaveDuree()
        {
            _fixture.GivenExistingRow(42, "riri");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenResults()[0].Duree.Should().Be(42);

            //BaseConfig config = GetConfig();
            //AddRow(42, "riri");

            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //traitement.Execute();
            //Assert.AreEqual(42, traitement.Items[0].Duree);
            //traitement.Items[0].Duree.Should().Be(42);
        }

        [TestMethod]
        public void ResultShouldHaveStoredProcedureName()
        {
            _fixture.GivenExistingRow(42, "riri");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenResults()[0].StoredProcedure.Should().Be("riri");

            //BaseConfig config = GetConfig();
            
            //AddRow(42, "riri");

            //TraitementDureeMoyenneRequeteSQL traitement = new TraitementDureeMoyenneRequeteSQL(_container);
            //traitement.Execute();
            //traitement.Items[0].StoredProcedure.Should().Be("riri");
        }

      

    }
}

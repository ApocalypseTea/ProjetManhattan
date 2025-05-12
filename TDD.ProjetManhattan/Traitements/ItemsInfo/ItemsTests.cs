using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProjetManhattan.Analyses;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using TDD.ProjetManhattan.Traitements.Items;

namespace TDD.ProjetManhattan.Traitements.ItemsInfo
{
    [TestClass]
    public class ItemsTests
    {
        private Fixture _fixture;
        

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
            
        }

        [TestMethod]
        public void ShouldHaveAnInstance()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");
            _fixture.WhenCreatingAnalyse();
            _fixture.ThenAnalyseInstance().Should().NotBeNull();
        }

        [TestMethod]
        public void CanUseProgramParameters()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");
            _fixture.WhenCreatingAnalyse();
            _fixture.ThenAnalyseInstance().Config.Should().BeOfType(typeof(BaseConfig));
            _fixture.ThenAnalyseInstance().Query.Should().BeOfType(typeof(string));
            _fixture.ThenAnalyseInstance().Input.Should().Be("baseDeDonnees.db");
        }

        [TestMethod]
        public void CanAccessToQueryInConfigFile()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");            
            _fixture.WhenCreatingAnalyse();
            _fixture.ThenAnalyseInstance().Query.Should().Be("SELECT DISTINCT target AS item\r\n    FROM record AS R\r\n    WHERE R.traitement IN ('LocalisationIp', 'StatIp')");
        }

        [TestMethod]
        public void HaveAccessToDatabase()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");
            _fixture.WhenCreatingAnalyse();
            _fixture.ThenAnalyseInstance().AccesSQLiteDB.Should().BeOfType(typeof(AccesDBSQLite));
        }

        [TestMethod]
        public void ShouldReturnOneResult()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");
            _fixture.GivenDatabaseResult("item1");
            _fixture.WhenExecutingAnalyse();
            _fixture.ThenAnalyseInstance()._lines.Should().HaveCount(1);
        }

        [TestMethod]
        public void ShouldReturnJsonObject()
        {
            _fixture.GivenInstanceParameters("baseDeDonnees.db", "query");
            _fixture.GivenDatabaseResult("item1");
            _fixture.WhenExecutingAnalyse();
            _fixture.ThenAnalyseInstance().ItemsInfoToJSON().Should().BeOfType(typeof(string));
            _fixture.ThenAnalyseInstance().ItemsInfoToJSON().Should().Contain("item1");

        }
    }
}

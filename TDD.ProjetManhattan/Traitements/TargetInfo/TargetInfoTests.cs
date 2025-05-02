using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ProjetManhattan.Configuration;
using ProjetManhattan.Traitements;
using Unity;
using ProjetManhattan;
using System.Data;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;


namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    [TestClass]
    public class TargetInfoTests
    {
        private Fixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
        }
        [TestMethod]
        public void ShouldHaveAName()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().Name.Should().Be("TargetInfo");
        } 

        [TestMethod]
        public void IsExistingConfigFile()
        {   _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenConfigFile().Should().NotBeNull();
        }

        [TestMethod]
        public void IsExistingViewDataInConfigFile()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenConfigFile()._jConfig["views"].Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void IsIPQueryViewFileExisting()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            File.Exists((string)_fixture.ThenConfigFile()._jConfig["views"]["ip"]["path"]).Should().Be(true);
        }

        [TestMethod]
        public void GetQueryFromConfigFilePath()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().Query.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void GetSQLQueryFromConfigFilePath()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().Query.Should().StartWith("SELECT");
            _fixture.ThenTraitementInstance().Query.Should().EndWith(";");
        }

        [TestMethod]
        public void ShouldHaveParameters()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.WhenTryingToConnectToDatabase();
            _fixture.ThenTraitementInstance().View.Should().Be("ip");
            _fixture.ThenTraitementInstance().Target.Should().Be("1.2.3.4");
            _fixture.ThenTraitementInstance().DateDebut.Should().Be(DateOnly.FromDateTime(new DateTime(2025, 03, 17)));
            _fixture.ThenTraitementInstance().DateFin.Should().Be(DateOnly.FromDateTime(new DateTime(2025, 03, 19)));
            _fixture.ThenTraitementInstance().AccesSQLiteDB.ConnectionString.Should().Be("Data Source=resultatTraitement.db");
        }

        [TestMethod]
        public void CanExecuteTraitement()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 42");
            _fixture.ThenTraitementInstance().Execute();
        }

        [TestMethod]
        public void CanConnectToSQLIteDatabase()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().AccesSQLiteDB.ConnexionBD().Should().BeOfType(typeof(SqliteConnection));
        }

        [TestMethod]
        public void CanExecuteAndReturnOneResult()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 42");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenTraitementInstance()._lines.Should().HaveCount(1);
        }

        [TestMethod]
        public void CanExecuteAndReturnRequeteTypeLine()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 42");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenTraitementInstance()._lines[0].Should().BeOfType(typeof(LigneRequeteSQLiteTargetInfo));
        }

        [TestMethod]
        public void CanReturnJSONwithData()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 42");
            _fixture.WhenExecutingTraitement();
            _fixture.ThenTraitementInstance().TargetInfoToJSON().Should().StartWith("[");
        }

        [TestMethod]
        public void CanVerifyJsonData()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), "resultatTraitement.db", new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 42");
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 56");
            _fixture.GivingExistingData("1.2.3.4", "'target', 1.2.3.4, 'pays', South Korea, 'nbRequetes', 24");
            _fixture.WhenExecutingTraitement();
            _fixture.WhenExecutingTargetInfoToJSON();
            ((_fixture.ThenJSONResult()[0]["target"] as JValue).Value as string).Should().Contain("1.2.3.4");
        }
    }
}

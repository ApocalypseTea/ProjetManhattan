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
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().Name.Should().Be("TargetInfo");
        } 


        [TestMethod]
        public void IsExistingConfigFile()
        {   _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025,03,17), new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenConfigFile().Should().NotBeNull();
        }

        [TestMethod]
        public void IsExistingViewDataInConfigFile()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenConfigFile()._jConfig["views"].Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void IsIPQueryViewFileExisting()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            File.Exists((string)_fixture.ThenConfigFile()._jConfig["views"]["ip"]["path"]).Should().Be(true);
        }

        [TestMethod]
        public void GetQueryFromConfigFilePath()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenExecutingTraitement();
            _fixture.ThenTraitementInstance().Query.Should().NotBeNullOrWhiteSpace();
        }

        [TestMethod]
        public void GetSQLQueryFromConfigFilePath()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenExecutingTraitement();
            _fixture.ThenTraitementInstance().Query.Should().StartWith("SELECT");
            _fixture.ThenTraitementInstance().Query.Should().EndWith(";");
        }

        [TestMethod]
        public void CanExecuteTraitement()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().Execute();
        }

        [TestMethod]
        public void CanConnectToSQLIteDatabase()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.GivingDatabasePath("resultatTraitement.db");
            _fixture.WhenStartingTraitementTargetInfo();
            _fixture.ThenTraitementInstance().AccesSQLiteDB.ConnectToDb().Should().BeOfType(typeof(SqliteConnection));
        }

        [TestMethod]
        public void Having3ParametersToQuery()
        {
            //_fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            //_fixture.GivingDatabasePath("resultatTraitement.db");
            //_fixture.WhenStartingTraitementTargetInfo();
            //_fixture.ThenTraitementInstance().GetSQLCommand(_fixture.ThenTraitementInstance().AccesSQLiteDB.ConnectToDb()).Parameters.Should().HaveCount(3);
            throw new NotImplementedException();
        }

        [TestMethod]
        public void ShouldReturnRecord()
        {
            _fixture.GivingTraitementParameters("ip", "1.2.3.4", new DateTime(2025, 03, 17), new DateTime(2025, 03, 19));
            _fixture.GivingDatabasePath("resultatTraitement.db");
        }



    }
}

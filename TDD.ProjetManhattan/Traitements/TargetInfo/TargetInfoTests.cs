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
        public void CanConnectToSQLiteDatabase()
        {
            throw new NotImplementedException();

        }

        [TestMethod]
        public void Having3ParametersToQuery()
        {
            throw new NotImplementedException();

        }

    }
}

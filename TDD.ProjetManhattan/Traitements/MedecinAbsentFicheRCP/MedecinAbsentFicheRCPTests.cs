using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;


namespace TDD.ProjetManhattan.Traitements.MedecinAbsentFicheRCP
{
    [TestClass]
    public class MedecinAbsentFicheRCPTests
    {
        Fixture _fixture;

        [TestInitialize]
        public void Init()
        {
            _fixture = new Fixture();
        }


        [TestMethod]
        public void CanCreateInstance()
        {
            _fixture.WhenCreatingTraitement();
            _fixture.ThenTraitement().Should().NotBeNull();
        }

        [TestMethod]
        public void ShouldHaveAName()
        {
            _fixture.WhenCreatingTraitement();
            _fixture.ThenTraitement().Name.Should().Be("MedecinParticipantAbsent");
        }

        [TestMethod]
        public void CanConnectToFakeDatabase()
        {
            _fixture.GivenExistingDataInDatabase(1, 23, DateTime.Now, 11256);
            _fixture.WhenExecuteTraitement();
            _fixture.FakeAccesBDD.ExpectData.Should().NotBeNull();
        }

        [TestMethod]
        public void CanExecuteTraitementAndAddItemsResults()
        {
            //Lui ajouter des fausses données (id reunion, id medecin, date, numeroFicheRCP)
            _fixture.GivenExistingDataInDatabase(1, 23, DateTime.Now, 11256);
            _fixture.WhenExecuteTraitement();
            _fixture.ThenTraitement()._lines.Count.Should().BeGreaterThanOrEqualTo(1);
        }
    }
}

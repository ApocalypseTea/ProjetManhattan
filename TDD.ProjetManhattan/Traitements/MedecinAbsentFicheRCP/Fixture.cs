using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using ProjetManhattan.Traitements;
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;
using Unity;

namespace TDD.ProjetManhattan.Traitements.MedecinAbsentFicheRCP
{
    internal class Fixture
    {
        private static readonly string FILENAME = "C:\\Users\\Adelas\\source\\repos\\ApocalypseTea\\ProjetManhattan\\ProjetManhattan\\Ressources\\config.json";

        private TraitementMedecinAbsentFicheRCPSQL _traitement;
        private IUnityContainer _container;
        public FakeAccesBDD FakeAccesBDD { get; }

        public Fixture()
        {
            _container = new UnityContainer();
            FakeAccesBDD = new FakeAccesBDD();
            _container.RegisterInstance<IUnityContainer>(_container);
            _container.RegisterInstance<IAccesBDD>(FakeAccesBDD);
        }

        private static BaseConfig GetConfig()
        {
            return new BaseConfig(FILENAME);
        }

        internal void GivenExistingDataInDatabase(int idReunion, int idMedecin, DateTime dateValidation, int idFicheRCP)
        {
            AddFakeDataBaseResult(idReunion, idMedecin, dateValidation, idFicheRCP);
        }

        private void AddFakeDataBaseResult(int idReunion, int idMedecin, DateTime dateValidation, int idFicheRCP)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("reunion_rcp", typeof(long)));
            dataTable.Columns.Add(new DataColumn("id_medecin", typeof(long)));
            dataTable.Columns.Add(new DataColumn("date_validation_fiche", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("fiche_rcp", typeof(long)));
            FakeAccesBDD.ExpectData = dataTable;
            FakeAccesBDD.ExpectData.Rows.Add(idReunion, idMedecin, dateValidation, idFicheRCP);
        }

        internal TraitementMedecinAbsentFicheRCPSQL ThenTraitement()
        {
            return _traitement;
        }

        internal void WhenCreatingTraitement()
        {
            BaseConfig config = GetConfig();
            _traitement = new TraitementMedecinAbsentFicheRCPSQL(_container, config);
        }

        internal void WhenExecuteTraitement()
        {
            WhenCreatingTraitement();
            _traitement.Execute();
        }
    }
}

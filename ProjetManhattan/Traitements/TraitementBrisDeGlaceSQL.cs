using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using Microsoft.Data.SqlClient;
using System.Data;
using ProjetManhattan.Formatages;
using System.Numerics;
using System.Reflection;
using Unity;

namespace ProjetManhattan.Traitements
{
    class TraitementBrisDeGlaceSQL : BaseTraitementParRequeteSQL<LigneRequeteBrisGlace>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryBrisDeGlace.txt";

        private int _seuilAlerteBrisGlace;
        private DateTime _dateTraitement;

        public string Name => "BrisGlace"; 
      
        public TraitementBrisDeGlaceSQL(IUnityContainer container, BaseConfig config, IAccesBDD accesBDD, IFormatage outputDisplay ) : base(container)
        {
            _lines = new List<LigneRequeteBrisGlace>();
            _source = accesBDD;
            _sortie = outputDisplay;
        }
        protected override LigneRequeteBrisGlace ReadItem(IDataReader reader)
        {
            int colProfilRef = reader.GetOrdinal("profil_ref");
            int colPrenom = reader.GetOrdinal("prenom");
            int colNom = reader.GetOrdinal("nom");
            int colLabel = reader.GetOrdinal("specialite_label");
            int colSpecialiteValue = reader.GetOrdinal("specialite_value");
            int colValue = reader.GetOrdinal("profil_type_value");
            int colNbPatient = reader.GetOrdinal("nb_patient_brise_glace");
            int colDate = reader.GetOrdinal("date");

            long profilRef = reader.GetInt64(colProfilRef);
            string prenom = reader.GetString(colPrenom);
            string nom = reader.GetString(colNom);
            string specialiteLabel = reader.GetString(colLabel);
            string specialiteValue = reader.GetString(colSpecialiteValue);
            string profilTypeValue = reader.GetString(colValue);
            int nbPatientBrisGlace = reader.GetInt32(colNbPatient);
            DateTime date = reader.GetDateTime(colDate);

            LigneRequeteBrisGlace ligne = new LigneRequeteBrisGlace(profilRef, prenom, nom, specialiteLabel, specialiteValue, profilTypeValue, nbPatientBrisGlace, date);

            return ligne;
        }
        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand requete = connection.CreateCommand();
            requete.CommandText = GetSQLQuery(RESOURCENAME);
            requete.CommandType = CommandType.Text;

            //requete.Parameters.AddWithValue("@Seuil", _seuilAlerteBrisGlace);
            requete.AddParameterWithValue("@Seuil", _seuilAlerteBrisGlace);
            requete.AddParameterWithValue("@dateTraitement", _dateTraitement);

            return requete;
        }

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigBrisGlace c = config.GetConfigTraitement<ConfigBrisGlace>(nameof(TraitementBrisDeGlaceSQL));
            _seuilAlerteBrisGlace = c.SeuilAlerteBrisDeGlaceJournalierParUtilisateur;
            _dateTraitement = config.DateTraitement;
        }
    }
}
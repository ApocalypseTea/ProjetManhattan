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

namespace ProjetManhattan.Traitements
{
    class TraitementBrisDeGlaceSQL : BaseTraitementParRequeteSQL<LigneRequeteBrisGlace>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryBrisDeGlace.txt";

        private int _seuilAlerteBrisGlace;
        private DateTime _dateTraitement;

        public string Name 
        { 
            get 
            { 
                return "BrisGlace"; 
            } 
        }

        public TraitementBrisDeGlaceSQL(BaseConfig config) : base(config)
        {
            ConfigBrisGlace c = config.GetConfigTraitement<ConfigBrisGlace>(nameof(TraitementBrisDeGlaceSQL));
            _seuilAlerteBrisGlace = c.SeuilAlerteBrisDeGlaceJournalierParUtilisateur;
            _dateTraitement = config.DateTraitement;
            _items = new List<LigneRequeteBrisGlace> ();
            _source = new AccesBDD(config);
            _sortie = new OutputDisplay();           
        }
        protected override LigneRequeteBrisGlace ReadItem(SqlDataReader reader)
        {
            int colProfilRef = reader.GetOrdinal("profil_ref");
            int colPrenom = reader.GetOrdinal("prenom");
            int colNom = reader.GetOrdinal("nom");
            int colLabel = reader.GetOrdinal("label");
            int colValue = reader.GetOrdinal("value");
            int colNbPatient = reader.GetOrdinal("nb_patient_brise_glace");
            int colDate = reader.GetOrdinal("_date");

            long profilRef = reader.GetInt64(colProfilRef);
            string prenom = reader.GetString(colPrenom);
            string nom = reader.GetString(colNom);
            string label = reader.GetString(colLabel);
            string value = reader.GetString(colValue);
            int nbPatientBrisGlace = reader.GetInt32(colNbPatient);
            DateTime date = reader.GetDateTime(colDate);

            LigneRequeteBrisGlace ligne = new LigneRequeteBrisGlace(profilRef, prenom, nom, label, value, nbPatientBrisGlace, date);

            return ligne;
        }
        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            SqlCommand requete = new SqlCommand(GetSQLQuery(RESOURCENAME), connection);
            requete.Parameters.AddWithValue("@Seuil", _seuilAlerteBrisGlace);
            requete.Parameters.AddWithValue("@dateTraitement", _dateTraitement);

            return requete;
        }
    }
}
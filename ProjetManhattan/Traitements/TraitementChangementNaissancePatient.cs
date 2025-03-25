using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    class TraitementChangementNaissancePatient : BaseTraitementParRequeteSQL<LigneRequeteChangementNaissancePatient>, ITraitement
    {
        protected const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementDateNaissancePatient.txt";
        private DateTime _dateTraitement;
        public TraitementChangementNaissancePatient(BaseConfig config) : base(config)
        {
            _dateTraitement = config.dateTraitement;
        }
        
        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            SqlCommand requete = new SqlCommand(GetSQLQuery(RESSOURCENAME), connection);
            requete.Parameters.AddWithValue("@dateTraitement", _dateTraitement);
            return requete;
        }        

        protected override LigneRequeteChangementNaissancePatient ReadItem(SqlDataReader reader)
        {
            int colIdPatient = reader.GetOrdinal("id_patient");
            int colDateActuelle = reader.GetOrdinal("date_actuelle");
            int colDateAnterieure = reader.GetOrdinal("date_precedente");
            int colModificateurId = reader.GetOrdinal("modificateurID");
            int colModificateurNom = reader.GetOrdinal("modificateurNom");
            int colModificateurPrenom = reader.GetOrdinal("modificateurPrenom");
            int colModificateurType = reader.GetOrdinal("modificateurType");
            int colDateModification = reader.GetOrdinal("date_derniere_modification");

            long idPatient = reader.GetInt64(colIdPatient);
            DateTime dateActuelle = reader.GetDateTime(colDateActuelle);
            DateTime dateAnterieure = reader.GetDateTime(colDateAnterieure);
            long modificateurID = reader.GetInt64(colModificateurId);
            string modificateurNom = reader.GetString(colModificateurNom);
            string modificateurPrenom = reader.GetString(colModificateurPrenom);
            string modificateurType = reader.GetString(colModificateurType);
            DateTime dateModification = reader.GetDateTime(colDateModification);

            return new LigneRequeteChangementNaissancePatient(idPatient, dateActuelle, dateAnterieure, modificateurID, modificateurNom, modificateurPrenom, modificateurType, dateModification);
        }
    }
}

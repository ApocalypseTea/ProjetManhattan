using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    class TraitementChangementNaissancePatient : BaseTraitementParRequeteSQL<LigneRequeteChangementNaissancePatient>, ITraitement
    {
        protected const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementDateNaissancePatient.txt";
        public TraitementChangementNaissancePatient(BaseConfig config) : base(config)
        {
        }
        
        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(GetSQLQuery(RESSOURCENAME), connection);
        }        

        protected override LigneRequeteChangementNaissancePatient ReadItem(SqlDataReader reader)
        {
            int colIdPatient = reader.GetOrdinal("id_patient");
            int colDateActuelle = reader.GetOrdinal("date_actuelle");
            int colDateAnterieure = reader.GetOrdinal("date_origine");
            int colModificateurId = reader.GetOrdinal("modificateurID");
            int colModificateurNom = reader.GetOrdinal("modificateurNom");
            int colModificateurPrenom = reader.GetOrdinal("modificateurPrenom");
            int colModificateurType = reader.GetOrdinal("modificateurType");

            long idPatient = reader.GetInt64(colIdPatient);
            DateTime dateActuelle = reader.GetDateTime(colDateActuelle);
            DateTime dateAnterieure = reader.GetDateTime(colDateAnterieure);
            long modificateurID = reader.GetInt64(colModificateurId);
            string modificateurNom = reader.GetString(colModificateurNom);
            string modificateurPrenom = reader.GetString(colModificateurPrenom);
            string modificateurType = reader.GetString(colModificateurType);

            return new LigneRequeteChangementNaissancePatient(idPatient, dateActuelle, dateAnterieure, modificateurID, modificateurNom, modificateurPrenom, modificateurType);
        }
    }
}

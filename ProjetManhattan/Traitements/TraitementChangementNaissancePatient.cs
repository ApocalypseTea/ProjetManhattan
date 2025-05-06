using System.Data;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using Unity;

namespace ProjetManhattan.Traitements
{
    class TraitementChangementNaissancePatient : BaseTraitementParRequeteSQL<LigneRequeteChangementNaissancePatient>, ITraitement
    {
        protected const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementDateNaissancePatient.txt";
        private DateTime _dateTraitement;
        public string Name => "ModificationDateNaissance"; 
        
        
        public TraitementChangementNaissancePatient(BaseConfig config, IUnityContainer container) : base(container)
        {
            
        }
        
        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand requete = connection.CreateCommand();
            requete.CommandText = GetSQLQuery(RESSOURCENAME);
            requete.CommandType = CommandType.Text;
            requete.AddParameterWithValue("@dateTraitement", _dateTraitement);
            return requete;
        }        

        protected override LigneRequeteChangementNaissancePatient ReadItem(IDataReader reader)
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

        public void InitialisationConfig(BaseConfig config)
        {
            _dateTraitement = config.DateTraitement;
        }
    }
}

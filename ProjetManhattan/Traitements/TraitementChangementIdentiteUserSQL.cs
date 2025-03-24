using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using ProjetManhattan.Configuration;


namespace ProjetManhattan.Traitements
{
    class TraitementChangementIdentiteUserSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLChgtIdentiteUser>, ITraitement
    {
        private const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementIdentiteUser.txt";
        public TraitementChangementIdentiteUserSQL(BaseConfig config) : base(config)
        {
        }
        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(GetSQLQuery(RESSOURCENAME), connection);
        }
        protected override LigneRequeteSQLChgtIdentiteUser ReadItem(SqlDataReader reader)
        {
            int colId = reader.GetOrdinal("id");
            int colNomActuel = reader.GetOrdinal("nom_enCours");
            int colPrenomActuel = reader.GetOrdinal("prenom_enCours");
            int colNomPrecedent = reader.GetOrdinal("nom_Origine");
            int colPrenomPrecedent = reader.GetOrdinal("prenom_Origine");
            int colModificateurId = reader.GetOrdinal("modificateurID");
            int colDateModificationNom = reader.GetOrdinal("dateModification");

            long idUser = reader.GetInt64(colId);
            string nomUser = reader.GetString(colNomActuel);
            string prenomUser = reader.GetString(colPrenomActuel);
            string previousPrenomUser = reader.GetString(colPrenomPrecedent);
            string previousNomUser = reader.GetString(colNomPrecedent);
            long modificateurID;
            if (!reader.IsDBNull(colModificateurId))
            {
                modificateurID = reader.GetInt64(colModificateurId);
            } 
            else
            {
                modificateurID = 0;
            }

            DateTime dateModification;
            if(!reader.IsDBNull(colDateModificationNom))
            {
                dateModification = reader.GetDateTime(colDateModificationNom);
            }else
            {
                dateModification = DateTime.Now;
            }

                LigneRequeteSQLChgtIdentiteUser ligne = new LigneRequeteSQLChgtIdentiteUser(idUser, nomUser, prenomUser, previousNomUser, previousPrenomUser, dateModification, modificateurID);

            return ligne;
        }
    }
}

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
        private const string QUERY = "SELECT U.id, U.nom AS nom_enCours, U.prenom AS prenom_enCours, UPN.nom AS nom_Origine, UPN.prenom AS prenom_Origine FROM account.ZT_user AS U JOIN account.ZT_user_previous_name AS UPN ON U.id = UPN.user_ref WHERE REPLACE(U.nom COLLATE French_CI_AI, '-', ' ')  != REPLACE(UPN.nom, '-', ' ') AND REPLACE(U.prenom COLLATE French_CI_AI, '-', ' ') != REPLACE(UPN.prenom,'-', ' ') AND UPN.prenom NOT LIKE '%Interne%' AND U.nom NOT LIKE '%TESTPROD%' AND REPLACE(U.nom COLLATE French_CI_AI, '-', ' ') != REPLACE(UPN.prenom, '-', ' ') AND REPLACE(U.prenom COLLATE French_CI_AI, '-', ' ') != REPLACE(UPN.nom,'-', ' ');";

        public TraitementChangementIdentiteUserSQL(BaseConfig config) : base(config)
        {

        }

        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(QUERY, connection);
        }

        protected override LigneRequeteSQLChgtIdentiteUser ReadItem(SqlDataReader reader)
        {
            int colId = reader.GetOrdinal("id");
            int colNomActuel = reader.GetOrdinal("nom_enCours");
            int colPrenomActuel = reader.GetOrdinal("prenom_enCours");
            int colNomPrecedent = reader.GetOrdinal("nom_Origine");
            int colPrenomPrecedent = reader.GetOrdinal("prenom_Origine");

            long idUser = reader.GetInt64(colId);
            string nomUser = reader.GetString(colNomActuel);
            string prenomUser = reader.GetString(colPrenomActuel);
            string previousPrenomUser = reader.GetString(colPrenomPrecedent);
            string previousNomUser = reader.GetString(colNomPrecedent);


            LigneRequeteSQLChgtIdentiteUser ligne = new LigneRequeteSQLChgtIdentiteUser(idUser, nomUser, prenomUser,previousNomUser, previousPrenomUser, DateTime.Now, 0);

            return ligne;
        }
    }
}

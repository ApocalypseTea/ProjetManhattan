using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using ProjetManhattan.Configuration;
using Unity;


namespace ProjetManhattan.Traitements
{
    class TraitementChangementIdentiteUserSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLChgtIdentiteUser>, ITraitement
    {
        private const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementIdentiteUser.txt";
        private DateTime _dateTraitement;

        public string Name { get { return "ChangementIdentite"; } }
        public TraitementChangementIdentiteUserSQL(BaseConfig config, IUnityContainer container) : base(container)
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
        protected override LigneRequeteSQLChgtIdentiteUser ReadItem(IDataReader reader)
        {
            int colId = reader.GetOrdinal("id");
            int colNomActuel = reader.GetOrdinal("nom_enCours");
            int colPrenomActuel = reader.GetOrdinal("prenom_enCours");
            int colNomPrecedent = reader.GetOrdinal("nom_Origine");
            int colPrenomPrecedent = reader.GetOrdinal("prenom_Origine");
            int colModificateurId = reader.GetOrdinal("modificateurID");
            int colDateModificationNom = reader.GetOrdinal("dateModification");
            int colModificateurNom = reader.GetOrdinal("nom_modificateur");
            int colModificateurPrenom = reader.GetOrdinal("prenom_modificateur");
            int colModificateurType = reader.GetOrdinal("type_modificateur");

            long idUser = reader.GetInt64(colId);
            string nomUser = reader.GetString(colNomActuel);
            string prenomUser = reader.GetString(colPrenomActuel);
            string previousPrenomUser = reader.GetString(colPrenomPrecedent);
            string previousNomUser = reader.GetString(colNomPrecedent);
            
            long modificateurID;
            string nomModificateur;
            string prenomModificateur;
            string typeModificateur;
            if (!reader.IsDBNull(colModificateurId))
            {
                modificateurID = reader.GetInt64(colModificateurId);
                nomModificateur = reader.GetString(colModificateurNom);
                prenomModificateur = reader.GetString(colModificateurPrenom);
                typeModificateur = reader.GetString(colModificateurType);
            } 
            else
            {
                modificateurID = 0;
                nomModificateur = "";
                prenomModificateur = "";
                typeModificateur = "";
            }

            DateTime dateModification;
            if(!reader.IsDBNull(colDateModificationNom))
            {
                dateModification = reader.GetDateTime(colDateModificationNom);
            }else
            {
                dateModification = _dateTraitement;
            }

                LigneRequeteSQLChgtIdentiteUser ligne = new LigneRequeteSQLChgtIdentiteUser(idUser, nomUser, prenomUser, previousNomUser, previousPrenomUser, dateModification, modificateurID, nomModificateur, prenomModificateur, typeModificateur);

            return ligne;
        }

        public void InitialisationConfig(BaseConfig config)
        {
            _dateTraitement = config.DateTraitement;
        }
    }
}

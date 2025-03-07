using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    class TraitementValidationParInterneSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLValidationParInterne>, ITraitement
    {
        private const string QUERY = "SELECT FR.id AS numeroFiche, FR.patient_ref AS patient, FR.validateur_ref AS validateur, U.nom, U.prenom, FR.reunion_ref AS numeroRCP, FR.date_validation\r\nFROM dbo.fiches_rcp AS FR\r\nJOIN account.profil_professionnel_sante AS PPS ON PPS.profil_id = FR.validateur_ref \r\nJOIN account.profil_professionnel_sante_titre_enum AS PPST ON PPST.id = PPS.titre_ref\r\nJOIN account.ZT_profil AS P ON P.id = FR.validateur_ref\r\nJOIN account.ZT_user AS U ON U.id = P.user_ref\r\nWHERE FR.date_validation IS NOT NULL AND PPST.value = 'Interne';";
        public TraitementValidationParInterneSQL(BaseConfig config) : base(config)
        {
            //ConfigRCPValideParInterne c = config.GetConfigTraitement<ConfigRCPValideParInterne>(nameof(TraitementValidationParInterneSQL));

            _items = new List<LigneRequeteSQLValidationParInterne>();
            _source = new AccesBDD(config);
            _sortie = new OutputDisplay();
        }

        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(QUERY, connection);
        }

        protected override LigneRequeteSQLValidationParInterne ReadItem(SqlDataReader reader)
        {
            int colNumeroFiche = reader.GetOrdinal("numeroFiche");
            long numeroFicheRCP = reader.GetInt64(colNumeroFiche);

            int colPatient = reader.GetOrdinal("patient");
            long idPatient = reader.GetInt64(colPatient);

            int colValidateur = reader.GetOrdinal("validateur");
            long idValidateur = reader.GetInt64(colValidateur);

            int colNom = reader.GetOrdinal("nom");
            string nomValidateur = reader.GetString(colNom);

            int colPrenom = reader.GetOrdinal("prenom");
            string prenomValidateur = reader.GetString(colPrenom);

            int colRCP = reader.GetOrdinal("numeroRCP");
            long numeroRCP = reader.GetInt64(colRCP);

            int colDate = reader.GetOrdinal("date_validation");
            DateTime date = reader.GetDateTime(colDate);

            LigneRequeteSQLValidationParInterne ligne = new LigneRequeteSQLValidationParInterne(numeroFicheRCP, idPatient, idValidateur, nomValidateur, prenomValidateur, numeroRCP, date);


            return ligne;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    class TraitementValidateurRCPnonPresentSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLValidateurNonPresent>, ITraitement
    {
        //private const string QUERY = "SELECT FR.validateur_ref AS Validateur, FR.id AS FicheRCP, FR.date_validation AS DateValidationRCP, R.id AS ReunionRCP\r\nFROM dbo.fiches_rcp AS FR\r\nJOIN dbo.reunions AS R ON R.id = FR.reunion_ref\r\nWHERE validateur_ref IS NOT NULL\r\nAND validateur_ref NOT IN (\r\n\tSELECT responsable_ref \r\n\tFROM dbo.reunions AS R\r\n\tWHERE R.id = FR.reunion_ref\r\n\t\r\n\tUNION\r\n\tSELECT medecin_ref\r\n\tFROM dbo.reunions_participants AS RP\r\n\tWHERE RP.reunion_ref = FR.reunion_ref\r\n\r\n\tUNION\r\n\tSELECT RT.responsable_ref\r\n\tFROM dbo.reunions_template AS RT\r\n\tJOIN dbo.reunions AS R ON RT.id = R.reunion_template_ref\r\n\tWHERE R.id = FR.reunion_ref\t\r\n\r\n\tUNION\r\n\tSELECT RT.responsable2_ref\r\n\tFROM dbo.reunions_template AS RT\r\n\tJOIN dbo.reunions AS R ON RT.id = R.reunion_template_ref\r\n\tWHERE R.id = FR.reunion_ref\r\n\r\n\tUNION\r\n\tSELECT RT.responsable3_ref\r\n\tFROM dbo.reunions_template AS RT\r\n\tJOIN dbo.reunions AS R ON RT.id = R.reunion_template_ref\r\n\tWHERE R.id = FR.reunion_ref\r\n\r\n\tUNION\r\n\tSELECT RT.responsable4_ref\r\n\tFROM dbo.reunions_template AS RT\r\n\tJOIN dbo.reunions AS R ON RT.id = R.reunion_template_ref\r\n\tWHERE R.id = FR.reunion_ref\r\n\t)\r\n;";

        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryValidateurRCPAbsent.txt";
        public TraitementValidateurRCPnonPresentSQL(BaseConfig config) : base(config)
        {
        }

        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(GetSQLQuery(RESOURCENAME), connection);
        }

        protected override LigneRequeteSQLValidateurNonPresent ReadItem(SqlDataReader reader)
        {
            int _colValidateur = reader.GetOrdinal("Validateur");
            int _colIdFicheRCP = reader.GetOrdinal("FicheRCP");
            int _colDateFicheRCP = reader.GetOrdinal("DateValidationRCP");
            int _colIdReunionRCP = reader.GetOrdinal("ReunionRCP");

            long _validateur = reader.GetInt64(_colValidateur);
            long _idFicheRCP = reader.GetInt64(_colIdFicheRCP);
            long _idReunionRCP = reader.GetInt64(_colIdReunionRCP);
            DateTime _dateFicheRCP = reader.GetDateTime(_colDateFicheRCP);

            return new LigneRequeteSQLValidateurNonPresent(_validateur, _idFicheRCP, _idReunionRCP, _dateFicheRCP);
        }
    }
}

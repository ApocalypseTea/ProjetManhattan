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

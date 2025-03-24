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
            int _colDateReunionRCP = reader.GetOrdinal("date_reunion");
            int _colIdSpecialiteMedicale = reader.GetOrdinal("specialite_ref");
            int _colLieuReunion = reader.GetOrdinal("label");
            int _colNomValidateur = reader.GetOrdinal("validateur_nom");
            int _colPrenomValidateur = reader.GetOrdinal("validateur_prenom");
            int _colSpecialiteMedicale = reader.GetOrdinal("specialite_med");

            long _validateur = reader.GetInt64(_colValidateur);
            long _idFicheRCP = reader.GetInt64(_colIdFicheRCP);
            long _idReunionRCP = reader.GetInt64(_colIdReunionRCP);
            DateTime _dateFicheRCP = reader.GetDateTime(_colDateFicheRCP);
            DateTime _dateReunionRCP = reader.GetDateTime(_colDateReunionRCP);
            long _idSpecialiteMed = reader.GetInt32(_colIdSpecialiteMedicale);
            string _lieuReunion = reader.GetString(_colLieuReunion);
            string _nomValidateur = reader.GetString(_colNomValidateur);
            string _prenomValidateur = reader.GetString(_colPrenomValidateur);
            string _specialiteMedicale = reader.GetString(_colSpecialiteMedicale);


            return new LigneRequeteSQLValidateurNonPresent(_validateur, _idFicheRCP, _idReunionRCP, _dateFicheRCP, _dateReunionRCP, _colIdSpecialiteMedicale, _lieuReunion, _nomValidateur, _prenomValidateur, _specialiteMedicale);
        }
    }
}

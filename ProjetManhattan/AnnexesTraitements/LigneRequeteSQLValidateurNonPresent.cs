using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    class LigneRequeteSQLValidateurNonPresent : IToRecordable
    {
        private long _idValidateur;
        private long _idFicheRCP;
        private long _idReunionRCP;
        private DateTime _dateFicheRCP;
        private DateTime _dateReunionRCP;
        private long _idSpecialiteMedicale;
        private string _lieuReunion;
        private string _nomValidateur;
        private string _prenomValidateur;
        private string _specialiteMedicale;

        public LigneRequeteSQLValidateurNonPresent(long validateur, long idFicheRCP, long idReunionRCP, DateTime dateValidationRCP, DateTime dateReunionRCP, long idSpecialiteMedicale, string lieuReunion, string nomValidateur, string prenomValidateur, string specialiteMedicale)
        {
            _idValidateur = validateur;
            _idFicheRCP = idFicheRCP;
            _idReunionRCP = idReunionRCP;
            _dateFicheRCP = dateValidationRCP;
            _dateReunionRCP = dateReunionRCP;
            _idSpecialiteMedicale = idSpecialiteMedicale;
            _lieuReunion = lieuReunion;
            _nomValidateur = nomValidateur;
            _prenomValidateur = prenomValidateur;
            _specialiteMedicale = specialiteMedicale;
        }
        public Record[] ToRecords()
        {
            JObject jsonDescription = new JObject();
            jsonDescription.Add("reunionID", this._idReunionRCP);
            jsonDescription.Add("date", this._dateReunionRCP);
            jsonDescription.Add("lieu", this._lieuReunion);
            jsonDescription.Add("specialite", this._specialiteMedicale);

            Record record = new Record()
            {
                

                Traitement = "ValidateurAbsent",
                Date = this._dateFicheRCP,
                Target = $"FicheRCPID={this._idFicheRCP}",
                PropertyName = "ValidateurAbsent",
                Description = jsonDescription.ToString(),
                Value = $"ValidateurID={this._idValidateur}"
            };

            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}

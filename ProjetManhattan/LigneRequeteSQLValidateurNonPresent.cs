using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ValidateurAbsent",
                Date = this._dateFicheRCP,
                Target = $"idValidateur={this._idValidateur} {this._prenomValidateur} {this._nomValidateur}",
                PropertyName = "ValidateurAbsent",
                Description = $"IdRCP={this._idReunionRCP} Date={this._dateReunionRCP} Lieu={this._lieuReunion} Specialite={this._idSpecialiteMedicale} {this._specialiteMedicale}",
                Value = _idFicheRCP.ToString(),
            };
        }
    }
}

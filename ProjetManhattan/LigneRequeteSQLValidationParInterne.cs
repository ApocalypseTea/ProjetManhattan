namespace ProjetManhattan
{
    class LigneRequeteSQLValidationParInterne : IToRecordable
    {
        private long _numeroFicheRCP;
        private long _idPatient;
        private long _idValidateur;
        private string _nomValidateur;
        private string _prenomValidateur;
        private long _numeroRCP;
        private DateTime _date;
        private DateTime _dateReunionRCP;
        private long _idSpecialiteMedicale;
        private string _lieuReunionRCP;
        private string _specialiteMedicale;


        public LigneRequeteSQLValidationParInterne(long numeroFicheRCP, long idPatient, long idValidateur, string nomValidateur, string prenomValidateur, long numeroRCP, DateTime date, DateTime dateReunionRCP, long specialiteMedicale, string lieuReunion, string specialiteMed)
        {
            _numeroFicheRCP = numeroFicheRCP;
            _idPatient = idPatient;
            _idValidateur = idValidateur;
            _nomValidateur = nomValidateur;
            _prenomValidateur = prenomValidateur;
            _numeroRCP = numeroRCP;
            _date = date;
            _dateReunionRCP = dateReunionRCP;
            _idSpecialiteMedicale = specialiteMedicale;
            _lieuReunionRCP = lieuReunion;
            _specialiteMedicale = specialiteMed;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ValidationInterne",
                Date = this._date,
                Target = $"FicheRcpID={this._numeroFicheRCP}",
                PropertyName = "Validateur",
                Description = $"ReunionID={this._numeroRCP} Date={this._dateReunionRCP.ToString("dd-MM-yyyy")}, Lieu={this._lieuReunionRCP}, Specialite={this._idSpecialiteMedicale} {this._specialiteMedicale}",
                Value = $"{this._idValidateur}={this._nomValidateur} {this._prenomValidateur}"
            };
        }
    }
}

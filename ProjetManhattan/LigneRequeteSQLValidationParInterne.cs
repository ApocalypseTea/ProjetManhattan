namespace ProjetManhattan
{
    class LigneRequeteSQLValidationParInterne : IToRecordable
    {
        private long numeroFicheRCP;
        private long idPatient;
        private long idValidateur;
        private string nomValidateur;
        private string prenomValidateur;
        private long numeroRCP;
        private DateTime date;
        private DateTime dateReunionRCP;
        private long idSpecialiteMedicale;
        private string lieuReunionRCP;
        private string specialiteMedicale;


        public LigneRequeteSQLValidationParInterne(long numeroFicheRCP, long idPatient, long idValidateur, string nomValidateur, string prenomValidateur, long numeroRCP, DateTime date, DateTime dateReunionRCP, long specialiteMedicale, string lieuReunion, string specialiteMed)
        {
            this.numeroFicheRCP = numeroFicheRCP;
            this.idPatient = idPatient;
            this.idValidateur = idValidateur;
            this.nomValidateur = nomValidateur;
            this.prenomValidateur = prenomValidateur;
            this.numeroRCP = numeroRCP;
            this.date = date;
            this.dateReunionRCP = dateReunionRCP;
            this.idSpecialiteMedicale = specialiteMedicale;
            this.lieuReunionRCP = lieuReunion;
            this.specialiteMedicale = specialiteMed;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ValidationInterne",
                Date = this.date,
                Target = $"IdFicheRCP={this.numeroFicheRCP}",
                PropertyName = "Validateur",
                Description = $"IdReunion={this.numeroRCP} Date={this.dateReunionRCP}, Lieu={this.lieuReunionRCP}, Specialite={this.idSpecialiteMedicale} {this.specialiteMedicale}",
                Value = $"{this.idValidateur}={this.nomValidateur} {this.prenomValidateur}"
            };
        }
    }
}

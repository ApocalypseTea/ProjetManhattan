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

        public LigneRequeteSQLValidationParInterne(long numeroFicheRCP, long idPatient, long idValidateur, string nomValidateur, string prenomValidateur, long numeroRCP, DateTime date)
        {
            this.numeroFicheRCP = numeroFicheRCP;
            this.idPatient = idPatient;
            this.idValidateur = idValidateur;
            this.nomValidateur = nomValidateur;
            this.prenomValidateur = prenomValidateur;
            this.numeroRCP = numeroRCP;
            this.date = date;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "TraitementRCPvalideParInterne",
                Date = this.date,
                Target = $"FicheRCPID:{this.numeroFicheRCP}",
                PropertyName = $"Validateur",
                Description = $"Concerne la RCP {this.numeroRCP}",
                Value = this.idValidateur
            };
        }
    }
}

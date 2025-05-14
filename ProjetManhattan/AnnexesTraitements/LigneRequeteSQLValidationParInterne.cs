using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

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
        private string _profilValidateur;


        public LigneRequeteSQLValidationParInterne(long numeroFicheRCP, long idPatient, long idValidateur, string nomValidateur, string prenomValidateur, long numeroRCP, DateTime date, DateTime dateReunionRCP, long specialiteMedicale, string lieuReunion, string specialiteMed, string profilValidateur)
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
            _profilValidateur = profilValidateur;
        }
        public Record[] ToRecords()
        {
            JObject jsonDescription = new JObject();
            jsonDescription.Add("ficheRCPID", this._numeroFicheRCP);
            jsonDescription.Add("dateReunion", this._dateReunionRCP);
            jsonDescription.Add("lieu",this._lieuReunionRCP);
            jsonDescription.Add("specialite", this._specialiteMedicale);
            string identiteValidateur = $"{this._nomValidateur} {this._prenomValidateur}";
            jsonDescription.Add("validateur", identiteValidateur);
            jsonDescription.Add("validateurProfilType", this._profilValidateur);
            
            Record record = new Record()
            {
                Traitement = "ValidationInterne",
                Date = this._date,
                Target = $"FicheRcpID={this._numeroFicheRCP}",
                PropertyName = "Validateur",
                Description = jsonDescription.ToString(),
                Value = $"{this._idValidateur}"
            };

            Record[] tableauRecord = {record};
            return tableauRecord;
        }
    }
}

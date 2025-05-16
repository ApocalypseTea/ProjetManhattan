using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    class LigneRequeteChangementNaissancePatient : IToRecordable
    {
        private long _idPatient;
        private DateTime _dateActuelle;
        private DateTime _dateAnterieure;
        private long _profilModificateurID;
        private string _modificateurNom;
        private string _modificateurPrenom;
        private string _modificateurType;
        private DateTime _dateModification;

        public LigneRequeteChangementNaissancePatient(long idPatient, DateTime dateActuelle, DateTime dateAnterieure, long profilModificateurID, string modificateurNom, string modificateurPrenom, string modificateurType, DateTime dateModification)
        {
            _idPatient = idPatient;
            _dateActuelle = dateActuelle;
            _dateAnterieure = dateAnterieure;
            _profilModificateurID = profilModificateurID;
            _modificateurNom= modificateurNom;
            _modificateurPrenom= modificateurPrenom;
            _modificateurType= modificateurType;
            _dateModification= dateModification;
        }

        public Record[] ToRecords()
        {
            JObject json = new JObject();
            json.Add("previousDate", DateOnly.FromDateTime(this._dateAnterieure).ToString("dd-MM-yyyy"));
            string modificateur = $"{this._profilModificateurID} {this._modificateurNom} {this._modificateurPrenom}";
            json.Add("modificateur", modificateur);
            json.Add("modificateurType", this._modificateurType);

            Record record = new Record()
            {
                Traitement = "ModificationDateNaissance",
                Target = $"PatientID={this._idPatient}",
                Date = this._dateModification,
                Value = $"{this._dateActuelle.Date.ToString("dd-MM-yyyy")}",
                PropertyName = "NouvelleDateNaissance",
                Description = json.ToString()
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}

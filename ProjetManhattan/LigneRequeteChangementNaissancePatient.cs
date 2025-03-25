using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ModificationDateNaissance",
                Target = $"PatientId={this._idPatient}",
                Date = this._dateModification,
                Value = $"{this._dateActuelle.Date.ToString("dd-MM-yyyy")}",
                PropertyName = "Date de naissance modifiee",
                Description = $"Previous Date={DateOnly.FromDateTime(this._dateAnterieure).ToString("dd-MM-yyyy")} / Modificateur={this._profilModificateurID} {this._modificateurNom} {this._modificateurPrenom}, {this._modificateurType}"
            };
            
                
            
        }
    }
}

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

        public LigneRequeteChangementNaissancePatient(long idPatient, DateTime dateActuelle, DateTime dateAnterieure, long profilModificateurID, string modificateurNom, string modificateurPrenom, string modificateurType)
        {
            _idPatient = idPatient;
            _dateActuelle = dateActuelle;
            _dateAnterieure = dateAnterieure;
            _profilModificateurID = profilModificateurID;
            _modificateurNom= modificateurNom;
            _modificateurPrenom= modificateurPrenom;
            _modificateurType= modificateurType;
        }

        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ModificationDateNaissance",
                Target = $"PatientId:{this._idPatient}",
                Date = DateTime.Now,
                Value = $"{this._dateAnterieure} en {this._dateActuelle}" ,
                PropertyName = "Date de naissance modifiee",
                Description = $"modifié par {this._profilModificateurID} {this._modificateurNom} {this._modificateurPrenom}, {this._modificateurType} "
            };
            
                
            
        }
    }
}

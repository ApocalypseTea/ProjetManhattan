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
        //private long _profilModificateur;

        public LigneRequeteChangementNaissancePatient(long idPatient, DateTime dateActuelle, DateTime dateAnterieure)
        {
            _idPatient = idPatient;
            _dateActuelle = dateActuelle;
            _dateAnterieure = dateAnterieure;
            //Ajouter le profil du modificateur dans le constructeur
        }

        public Record ToRecord()
        {
            return new Record()
            {
                Traitement = "ModificationDateNaissance",
                Target = $"PatientId:{this._idPatient}",
                Date = DateTime.Now,
                Value = 0f,
                PropertyName = "Date de naissance modifiee",
                Description = $"{this._dateAnterieure} modifié par ? en {this._dateActuelle}"
            };
            
                
            
        }
    }
}

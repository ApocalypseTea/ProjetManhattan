using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    class LigneRequeteSQLChgtIdentiteUser : IToRecordable
    {
        private long _idUser;
        private string _nomUser;
        private string _prenomUser;
        private string _previousNomUser;
        private string _previousPrenomUser;
        private DateTime _dateModificationNom;
        private long _idModificateur;
        private string _nomModificateur;
        private string _prenomModificateur;
        private string _typeModificateur;

        public LigneRequeteSQLChgtIdentiteUser(long idUser, string nomUser, string prenomUser, string previousNomUser, string previousPrenomUser, DateTime dateModificationNom, long idModificateur, string nomModificateur, string prenomModificateur, string typeModificateur)
        {
            _idUser = idUser;
            _nomUser = nomUser;
            _prenomUser = prenomUser;
            _previousNomUser = previousNomUser;
            _previousPrenomUser = previousPrenomUser;
            _dateModificationNom = dateModificationNom;
            _idModificateur = idModificateur;
            _nomModificateur = nomModificateur;
            _prenomModificateur = prenomModificateur;
            _typeModificateur = typeModificateur;
        }
        public Record[] ToRecords()
        {
            JObject jobject = new JObject();
            string nomCompletPrevious = $"{this._previousNomUser} {this._previousPrenomUser}";
            jobject.Add("previousName", nomCompletPrevious);
            jobject.Add("modificateurID", this._idModificateur);
            string nomCompletModificateur = $"{this._nomModificateur} {this._prenomModificateur}";
            jobject.Add("modificateur", nomCompletModificateur);
            jobject.Add("modificateurType", this._typeModificateur);

            Record record = new Record()
            {
                Traitement = "ChangementIdentite",
                Date = this._dateModificationNom,
                Target = $"UserID={this._idUser}",
                PropertyName = $"Modificateur",
                Description = jobject.ToString(),
                Value = $"{this._prenomUser} {this._nomUser}"
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}

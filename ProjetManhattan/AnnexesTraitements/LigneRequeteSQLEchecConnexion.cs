using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan.Traitements
{
    public class LigneRequeteSQLEchecConnexion : IToRecordable
    {
        private long _credentials;
        private long _idUser;
        private string _nom;
        private string _prenom;
        private int _nbTentatives;
        private DateTime _dateTraitement;
        private string _profilType;
        private string _specialiteValue;
        private string _specialiteLabel;

        public LigneRequeteSQLEchecConnexion(long credentials, long idUser, string nom, string prenom, int nbTentatives, DateTime dateTraitement, string profilType, string specialiteValue, string specialiteLabel)
        {
            _credentials = credentials;
            _idUser = idUser;
            _nom = nom;
            _prenom = prenom;
            _nbTentatives = nbTentatives;
            _dateTraitement = dateTraitement;
            _profilType = profilType;
            _specialiteValue = specialiteValue;
            _specialiteLabel = specialiteLabel;
        }

        public Record[] ToRecords()
        {
            JObject jObject = new JObject();
            jObject.Add("nom", this._nom);
            jObject.Add("prenom", this._prenom);
            jObject.Add("profilID", this._idUser);
            jObject.Add("profilType", this._profilType);
            jObject.Add("specialite", this._specialiteValue);
            jObject.Add("specialiteLabel", this._specialiteLabel);
            Record record = new Record()
            {
                Target = this._idUser.ToString(),
                Value = this._nbTentatives.ToString(),
                Date = _dateTraitement,
                PropertyName = "ConnectionAttempt",
                Traitement = "EchecConnexion",
                Description = jObject.ToString(),
            };

            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}
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

        public LigneRequeteSQLEchecConnexion(long credentials, long idUser, string nom, string prenom, int nbTentatives, DateTime dateTraitement)
        {
            _credentials = credentials;
            _idUser = idUser;
            _nom = nom;
            _prenom = prenom;
            _nbTentatives = nbTentatives;
            _dateTraitement = dateTraitement;
        }

        public Record[] ToRecords()
        {
            Record record = new Record()
            {
                Target = this._idUser.ToString(),
                Value = this._nbTentatives.ToString(),
                Date = _dateTraitement,
                PropertyName = "ConnectionAttempt",
                Traitement = "EchecConnexion",
                Description = $"{_nom} {_prenom}"
            };

            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}
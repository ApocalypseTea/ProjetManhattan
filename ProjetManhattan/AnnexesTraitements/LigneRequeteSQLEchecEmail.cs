using ProjetManhattan.Formatages;

namespace ProjetManhattan.AnnexesTraitements
{
    internal class LigneRequeteSQLEchecEmail : IToRecordable
    {
        private int _nbMailsEnEchec;
        private string _typeMailsEnEchec;
        private DateTime _dateEnvoiMails;
        public LigneRequeteSQLEchecEmail(int nbMails, string typeMails, DateTime dateEnvoiMail)
        {
            _dateEnvoiMails = dateEnvoiMail;
            _nbMailsEnEchec = nbMails;
            _typeMailsEnEchec = typeMails;
        }

        public Record[] ToRecords()
        {
            Record record = new Record()
            {
                Target = _typeMailsEnEchec,
                Value = this._nbMailsEnEchec.ToString(),
                Date = _dateEnvoiMails,
                PropertyName = "NbMailEnEchec",
                Traitement = "EchecEmail",
                Description = ""
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}
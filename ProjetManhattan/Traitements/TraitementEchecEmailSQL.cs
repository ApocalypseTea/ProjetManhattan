using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.AnnexesTraitements;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    internal class TraitementEchecEmailSQL : BaseTraitementParRequeteSQL<LigneRequeteSQLEchecEmail>, ITraitement
    {
        private const string RESOURCENAME = "ProjetManhattan.Configuration.QueryEchecEmailSQL.txt";

        private int _seuilAlerteEmail;
        private DateTime _dateTraitement;
        public string Name => "EchecEmail";

        public TraitementEchecEmailSQL(IUnityContainer container, BaseConfig config, IAccesBDD accesBDD, IFormatage outputDisplay) : base(container)
        {
            _lines = new List<LigneRequeteSQLEchecEmail>();
            _source = accesBDD;
            _sortie = outputDisplay;
        }

        public void InitialisationConfig(BaseConfig config)
        {
            ConfigEchecEmail c = config.GetConfigTraitement<ConfigEchecEmail>(nameof(TraitementEchecEmailSQL));
            _seuilAlerteEmail = c.SeuilAlerteNbEmailsEnEchec;
            _dateTraitement = config.DateTraitement;
        }

        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            IDbCommand requete = connection.CreateCommand();
            requete.CommandText = GetSQLQuery(RESOURCENAME);
            requete.CommandType = CommandType.Text;
            requete.AddParameterWithValue("@Seuil", _seuilAlerteEmail);
            requete.AddParameterWithValue("@Date", _dateTraitement);

            return requete;
        }

        protected override LigneRequeteSQLEchecEmail ReadItem(IDataReader reader)
        {
            int colNbMails = reader.GetOrdinal("nb_mails");
            int colTypeMail = reader.GetOrdinal("value");
            int colDateEnvoi = reader.GetOrdinal("date_envoi");

            int nbMails = reader.GetInt32(colNbMails);
            string typeMails = reader.GetString(colTypeMail);
            DateTime date = reader.GetDateTime(colDateEnvoi);

            LigneRequeteSQLEchecEmail ligne = new LigneRequeteSQLEchecEmail(nbMails, typeMails, date);

            return ligne;
        }
    }
}

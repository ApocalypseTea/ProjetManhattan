using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;
using Unity;

namespace ProjetManhattan.Traitements
{
    public class TraitementMedecinAbsentFicheRCPSQL : BaseTraitementParRequeteSQL<LigneRequeteMedecinAbsentFicheRCP>, ITraitement
    {
        private static readonly string RESOURCENAME = "ProjetManhattan.Configuration.QueryMedecinParticipantReunionAbsentFicheRCP.txt";
        private DateTime _dateTraitement;

        public TraitementMedecinAbsentFicheRCPSQL(IUnityContainer container, BaseConfig config) : base(container)
        {
           
        }

        public string Name => "MedecinParticipantAbsent";

        public void InitialisationConfig(BaseConfig config)
        {
            _dateTraitement = config.DateTraitement;
        }

        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            using (IDbCommand commande = connection.CreateCommand())
            {
                //Console.WriteLine("Je suis une instance de tttMedecinAbsent : ", _dateTraitement);
                commande.CommandText = GetSQLQuery(RESOURCENAME);
                commande.CommandType = CommandType.Text;
                commande.AddParameterWithValue("@DateValidation", _dateTraitement);

                return commande;
            }
        }

        protected override LigneRequeteMedecinAbsentFicheRCP ReadItem(IDataReader reader)
        {
            int colReunionId = reader.GetOrdinal("reunion_rcp");
            int colMedecinId = reader.GetOrdinal("id_medecin");
            int colDateValidation = reader.GetOrdinal("date_validation_fiche");
            int colFicheRcpId = reader.GetOrdinal("fiche_rcp");

            long idReunion = reader.GetInt64(colReunionId);
            long idMedecin=reader.GetInt64(colMedecinId);
            long idFicheRcp = reader.GetInt64(colFicheRcpId);
            DateTime dateValidation = reader.GetDateTime(colDateValidation);

            return new LigneRequeteMedecinAbsentFicheRCP(idReunion, idMedecin, dateValidation, idFicheRcp);
        }
    }
}

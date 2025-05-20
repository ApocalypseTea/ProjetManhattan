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
    public class TraitementEchecConnexion : BaseTraitementParRequeteSQL<LigneRequeteSQLEchecConnexion>, ITraitement
    {
        private readonly string RESSOURCE = "ProjetManhattan.Configuration.QueryEchecConnexion.txt";
        private DateTime _date;
        private int _nbConnexionMini;
        public string Name => "EchecConnexion";
        public TraitementEchecConnexion(IUnityContainer container, BaseConfig config) : base(container)
        {
        }

        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            using (IDbCommand commande = connection.CreateCommand())
            {
                //Console.WriteLine("Je suis une instance de tttMedecinAbsent : ", _dateTraitement);
                commande.CommandText = GetSQLQuery(RESSOURCE);
                commande.CommandType = CommandType.Text;
                commande.AddParameterWithValue("@date", _date);
                commande.AddParameterWithValue("@nbTentative", _nbConnexionMini);

                return commande;
            }
        }

        protected override LigneRequeteSQLEchecConnexion ReadItem(IDataReader reader)
        {
            int colCredentials = reader.GetOrdinal("credential");
            int colIdUser = reader.GetOrdinal("id_user");
            int colNom = reader.GetOrdinal("nom");
            int colPrenom = reader.GetOrdinal("prenom");
            int colNbConnexion = reader.GetOrdinal("nb_connexion");
            int colProfilType = reader.GetOrdinal("profil_type");
            int colSpecialiteValue = reader.GetOrdinal("specialite_value");
            int colSpecialiteLabel = reader.GetOrdinal("specialite_label");

            long credential = reader.GetInt64(colCredentials);
            long idUser = reader.GetInt64(colIdUser);
            string nomUser = reader.GetString(colNom);
            string prenomUser = reader.GetString(colPrenom);
            int nbTentativesConnexion = reader.GetInt32(colNbConnexion);
            string profilType = reader.GetString(colProfilType);
            string specialiteValue = reader.GetString(colSpecialiteValue);
            string specialiteLabel = reader.GetString(colSpecialiteLabel);

            return new LigneRequeteSQLEchecConnexion(credential, idUser, nomUser, prenomUser, nbTentativesConnexion, _date, profilType, specialiteValue, specialiteLabel);
        }

        public void InitialisationConfig(BaseConfig config)
        {
            _date = config.DateTraitement;
            ConfigEchecConnexion c = config.GetConfigTraitement<ConfigEchecConnexion>(nameof(TraitementEchecConnexion));
            _nbConnexionMini = c.SeuilAlerteTentativesConnexionEnEchec;
        }
    }
}

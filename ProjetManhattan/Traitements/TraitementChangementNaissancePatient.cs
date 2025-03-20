using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    class TraitementChangementNaissancePatient : BaseTraitementParRequeteSQL<LigneRequeteChangementNaissancePatient>, ITraitement
    {
        private const string QUERY = "SELECT I.id AS id_patient, I.date_naissance AS date_actuelle, IH.date_naissance AS date_precedente\r\nFROM patient.ZT_identite_history AS IH\r\nLEFT JOIN patient.ZT_identite AS I ON I.id=IH.id\r\nWHERE I.date_naissance != IH.date_naissance;";
        private const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementDateNaissancePatient.txt";
        public TraitementChangementNaissancePatient(BaseConfig config) : base(config)
        {
        }
        
        protected override SqlCommand GetSQLCommand(SqlConnection connection)
        {
            return new SqlCommand(GetSQLQuery(RESSOURCENAME), connection);
        }        

        protected override LigneRequeteChangementNaissancePatient ReadItem(SqlDataReader reader)
        {
            int colIdPatient = reader.GetOrdinal("id_patient");
            int colDateActuelle = reader.GetOrdinal("date_actuelle");
            int colDateAnterieure = reader.GetOrdinal("date_origine");

            long idPatient = reader.GetInt64(colIdPatient);
            DateTime dateActuelle = reader.GetDateTime(colDateActuelle);
            DateTime dateAnterieure = reader.GetDateTime(colDateAnterieure);

            return new LigneRequeteChangementNaissancePatient(idPatient, dateActuelle, dateAnterieure);
        }
    }
}

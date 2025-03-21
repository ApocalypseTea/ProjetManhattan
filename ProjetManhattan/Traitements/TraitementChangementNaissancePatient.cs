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
        protected const string RESSOURCENAME = "ProjetManhattan.Configuration.QueryChangementDateNaissancePatient.txt";
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
            int colModificateur = reader.GetOrdinal("modificateur");

            long idPatient = reader.GetInt64(colIdPatient);
            DateTime dateActuelle = reader.GetDateTime(colDateActuelle);
            DateTime dateAnterieure = reader.GetDateTime(colDateAnterieure);
            long modificateur = reader.GetInt64(colModificateur);

            return new LigneRequeteChangementNaissancePatient(idPatient, dateActuelle, dateAnterieure, modificateur);
        }
    }
}

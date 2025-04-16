using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    public class TraitementDureeMoyenneRequeteSQL : BaseTraitementParRequeteSQL<LigneRequeteDureeMoyenneSQL>, ITraitement
    {
        private readonly static string RESOURCENAME = "ProjetManhattan.Configuration.QueryDureeRequeteMoyenneSQL.txt";
        private IAccesBDD _accesBDD;
        public string Name => "DureeTraitementRequeteSQL";

        public TraitementDureeMoyenneRequeteSQL(IUnityContainer container) : base(container)
        {

        }

        public void Display(string exportDataMethod, string nomDB)
        {
            throw new NotImplementedException();
        }

        //public void Execute()
        //{
        //    IAccesBDD acces = this.Container.Resolve<IAccesBDD>();
        //    using (IDbConnection connection = acces.ConnexionBD())
        //    using(IDbCommand cmd = connection.CreateCommand())
        //    {
        //        cmd.CommandText = GetSQLQuery(RESOURCENAME);
        //        cmd.CommandType = CommandType.Text;
        //    }
        //}

        protected override LigneRequeteDureeMoyenneSQL ReadItem(IDataReader reader)
        {
            int colAvgElapsedTime = reader.GetOrdinal("avg_elapsed_time");
            int colStoredProcedure = reader.GetOrdinal("stored_procedure");
            //int colPrenom = reader.GetOrdinal("prenom");
            //int colNom = reader.GetOrdinal("nom");
            //int colLabel = reader.GetOrdinal("label");
            //int colValue = reader.GetOrdinal("value");
            //int colNbPatient = reader.GetOrdinal("nb_patient_brise_glace");
            //int colDate = reader.GetOrdinal("date");

            float duree = reader.GetFloat(colAvgElapsedTime);
            string storedProcedure = reader.GetString(colStoredProcedure);

            //string prenom = reader.GetString(colPrenom);
            //string nom = reader.GetString(colNom);
            //string label = reader.GetString(colLabel);
            //string value = reader.GetString(colValue);
            //int nbPatientBrisGlace = reader.GetInt32(colNbPatient);
            //DateTime date = reader.GetDateTime(colDate);

            LigneRequeteDureeMoyenneSQL ligne = new LigneRequeteDureeMoyenneSQL(duree, storedProcedure);

            return ligne;
        }

        protected override IDbCommand GetSQLCommand(IDbConnection connection)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = GetSQLQuery(RESOURCENAME);
                cmd.CommandType = CommandType.Text;
                return cmd;
            }
        }

        public IReadOnlyList<LigneRequeteDureeMoyenneSQL> Items => _lines;
    }
}

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
        public string Name => "DureeMoyRequeteSQL";

        public TraitementDureeMoyenneRequeteSQL(IUnityContainer container) : base(container)
        {
            //Console.WriteLine("Je suis une instance de Ttt duree moyenne Requete sql");
        }

        protected override LigneRequeteDureeMoyenneSQL ReadItem(IDataReader reader)
        {
            int colAvgElapsedTime = reader.GetOrdinal("avg_elapsed_time");
            int colStoredProcedure = reader.GetOrdinal("stored_procedure");
            int colExecutionCount = reader.GetOrdinal("execution_count");

            long duree = reader.GetInt64(colAvgElapsedTime);
            long nbExecution = reader.GetInt64(colExecutionCount);
            
            string storedProcedure;
            if (!reader.IsDBNull(colStoredProcedure))
            {
                storedProcedure = reader.GetString(colStoredProcedure);
            }
            else
            {
                storedProcedure = "NULL";
            }
            
            LigneRequeteDureeMoyenneSQL ligne = new LigneRequeteDureeMoyenneSQL(duree, storedProcedure, nbExecution);

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
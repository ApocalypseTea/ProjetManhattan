using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    public class LigneRequeteDureeMoyenneSQL : IToRecordable
    {
        public long Duree { get; set; }
        public string StoredProcedure { get; set; }
        public long nbExecution { get; set; }

        public LigneRequeteDureeMoyenneSQL(long duree, string storedProcedure, long nbExecution)
        {
            this.Duree = duree;
            this.StoredProcedure = storedProcedure;
            this.nbExecution = nbExecution;
        }
        public Record[] ToRecords()
        {
            Record record1 = new Record()
            {
                Target = this.StoredProcedure,
                Value = this.Duree.ToString(),
                Date = DateTime.Now,
                PropertyName = "AverageDuration",
                Traitement = "DureeMoyRequeteSQL",
                Description = "{}"
            };

            Record record2 = new Record()
            {
                Target = this.StoredProcedure,
                Value = this.nbExecution.ToString(),
                Date = DateTime.Now,
                PropertyName = "ExecutionCount",
                Traitement = "DureeMoyRequeteSQL",
                Description = "{}"
            };

            Record[] tableauRecord = { record1, record2 };
            return tableauRecord;
        }
    }
}

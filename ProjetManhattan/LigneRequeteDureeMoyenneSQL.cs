using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    public class LigneRequeteDureeMoyenneSQL : IToRecordable
    {
        public float Duree { get; set; }
        public string StoredProcedure { get; set; }

        public LigneRequeteDureeMoyenneSQL(float duree, string storedProcedure)
        {
            this.Duree = duree;
            this.StoredProcedure = storedProcedure;
        }
        public Record ToRecord()
        {
            return new Record()
            {
                Target = this.StoredProcedure,
                Value = this.Duree.ToString(),
                Date = DateTime.Now,
                PropertyName = "AverageDuration",
                Traitement = "DureeTraitementRequeteSQL"
            };
        }
    }
}

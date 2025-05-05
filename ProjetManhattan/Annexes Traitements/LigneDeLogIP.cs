using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    class LigneDeLogIP : LigneDeLog, IToRecordable
    {
        LigneDeLogIP(string[] champsLog): base(champsLog)
        {
        }

        public Record[] ToRecords()
        {
            Record record = new Record()
            {
                Target = this.IpClient,
                Date = this.DateHeure
            };
            Record[] tableauRecord = { record };
            return tableauRecord;
        }
    }
}

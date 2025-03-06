using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class LigneDeLogIP : LigneDeLog, IToRecordable
    {
        LigneDeLogIP(string[] champsLog): base(champsLog)
        {

        }

        public Record ToRecord()
        {
            return new Record()
            {
                Target = this.IpClient,
                Date = this.DateHeure
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Formatages
{
    internal interface IFormatage
    {
        //void AffichageNotifications(IEnumerable<Notification> notifications);

        void AffichageRecord(IEnumerable<Record> listeDeRecord);
    }
}

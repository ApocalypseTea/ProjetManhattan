using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Formatages
{
    internal class OutputDisplay : IFormatage
    {
        //public void AffichageNotifications(IEnumerable<Notification> notifications)
        //{
        //    foreach(Notification notification in notifications)
        //    {
        //        Console.WriteLine(notification.Message);
        //    }
        //}

        public void AffichageRecord(IEnumerable<Record> listeDeRecord)
        {
            foreach(Record record in listeDeRecord)
            {
                Console.WriteLine($"{ record.Traitement} : {record.Target} : {record.Date} : {record.Value} : {record.PropertyName} : {record.Description}");
            }
        }
    }
}

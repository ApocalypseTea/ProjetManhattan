using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Formatages
{
    public class OutputDisplay : IFormatage
    {
        public void AffichageRecord(Record record)
        {
            Console.WriteLine($"{ record.Traitement} : {record.Target} : {record.Date.Value.ToString("dd-MM-yyyy")} : {record.Value} : {record.PropertyName} : {record.Description}");
        }
    }
}

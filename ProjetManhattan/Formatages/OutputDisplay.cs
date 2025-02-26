using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Formatages
{
    internal class OutputDisplay : IFormatage
    {
        public void Display(IEnumerable<Notification> notifications)
        {
            foreach(Notification notification in notifications)
            {
                Console.WriteLine(notification.Message);
            }
        }
    }
}

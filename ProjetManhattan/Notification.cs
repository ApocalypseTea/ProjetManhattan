using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class Notification
    {
        private string _message;

        public Notification(string message) 
        {
            _message = message;
        }

        public string Message => _message;
       
    }
}

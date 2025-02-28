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
        private int? _priority;

        public Notification(string message, int? priority = null) 
        {
            _message = message;
            _priority = priority;
        }
        public string Message => _message;   
        public int? Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
    }
}

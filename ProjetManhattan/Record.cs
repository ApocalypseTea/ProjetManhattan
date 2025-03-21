using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    class Record
    {
        public string Traitement { get; set; }
        public string Target { get; set; }
        public DateTime? Date { get; set; }
        public string Value { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
    }
}

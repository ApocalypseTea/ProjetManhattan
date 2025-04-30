using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetManhattan.Formatages
{
    class ZabbixData
    {
        //[JsonProperty("target")]
        public string Target {  get; set; }
        public string Traitement { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public ZabbixData(string target, string traitement, string propertyName, string description, DateTime date)
        {
            Target = target;
            Traitement = traitement;
            PropertyName = propertyName;
            Description = description;
            Date = date;
        }
    }
}

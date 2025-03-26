using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetManhattan
{
    class ZabbixData
    {
        //[JsonProperty("target")]
        public string Target {  get; set; }
        public string Traitement { get; set; }
        public string PropertyName { get; set; }
        public string Description { get; set; }
        public ZabbixData(string target, string traitement, string propertyName, string description)
        {
            Target = target;
            Traitement = traitement;
            PropertyName = propertyName;
            Description = description;
        }
    }
}

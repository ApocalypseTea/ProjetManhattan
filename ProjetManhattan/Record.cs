using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProjetManhattan
{
    class Record
    {
        [JsonProperty("traitement")]
        public string Traitement { get; set; }
        [JsonProperty("target")]
        public string Target { get; set; }
        [JsonProperty("date")]
        public DateTime? Date { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        
        [JsonProperty("propertyName")]
        public string PropertyName { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}

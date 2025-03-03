using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjetManhattan.Configuration
{
    class ConfigStatsIP
    {
        //[JsonProperty("toto")]
        public HashSet<string> adressesIPValides { get; init; }

        public int seuilAlerteRequetesParIp { get; init; }       
    }
}

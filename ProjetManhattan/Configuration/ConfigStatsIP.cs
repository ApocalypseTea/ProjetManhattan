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
        public HashSet<string> AdressesIPValides { get; init; }
        public int SeuilAlerteRequetesParIp { get; init; }       
    }
}

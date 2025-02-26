using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    internal class TempsRequete
    {
        public int timeTaken { get; init; }
        public string url { get; init; }
        public int timeQuery { get; init; }
        public IpClient ipClient { get; init; }

        public TempsRequete(IpClient iPClient, int timeTaken, string url, int timeQuery)
        {
            this.ipClient = iPClient;
            this.timeTaken = timeTaken;
            this.url = url;
            this.timeQuery = timeQuery;
        }
    }
}

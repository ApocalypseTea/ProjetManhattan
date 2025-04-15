using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    public class TempsRequete
    {
        public int TimeTaken { get; init; }
        public string Url { get; init; }
        public int TimeQuery { get; init; }
        public IpClient IpClient { get; init; }

        public TempsRequete(IpClient iPClient, int timeTaken, string url, int timeQuery)
        {
            this.IpClient = iPClient;
            this.TimeTaken = timeTaken;
            this.Url = url;
            this.TimeQuery = timeQuery;
        }
    }
}

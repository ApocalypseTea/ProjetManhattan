using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    public class LigneRequeteSQLiteTargetInfo
    {
        public string Target { get; init; }

        public string Json { get; init; }

        public LigneRequeteSQLiteTargetInfo(string target, string json)
        {
            Target = target;
            Json = json;
        }

        
    }
}
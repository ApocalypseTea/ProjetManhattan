using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using ProjetManhattan.Formatages;

namespace ProjetManhattan
{
    public class LigneRequeteSQLiteTargetInfo : IToRecordable
    {
        private string _target;
        private string _json;

        public LigneRequeteSQLiteTargetInfo(string target, string json)
        {
            _target = target;
            _json = json;
        }

        public Record ToRecord()
        {
            throw new NotImplementedException();
        }
    }
}
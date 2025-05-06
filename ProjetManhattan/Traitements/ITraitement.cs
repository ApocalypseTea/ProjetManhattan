using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Configuration;

namespace ProjetManhattan.Traitements
{
    public interface ITraitement
    {
        string Name { get; }

        void InitialisationConfig(BaseConfig config);
        void Display(string exportDataMethod, string nomDB);
        void Execute();
    }
}

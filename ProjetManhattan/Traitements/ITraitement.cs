using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Traitements
{
    internal interface ITraitement
    {
        void Display(int exportDataMethod);
        void Execute();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    internal interface IFiltre
    {
        bool Needed(LigneDeLog ligne);
    }
}

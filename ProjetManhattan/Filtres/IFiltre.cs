using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Filtres
{
    public interface IFiltre
    {
        bool Needed(LigneDeLog ligne);
    }
}

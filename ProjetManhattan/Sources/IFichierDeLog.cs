﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan.Sources
{
    internal interface IFichierDeLog : ISource
    {
        bool HasLines();
        LigneDeLog? ReadLine();
    }
}

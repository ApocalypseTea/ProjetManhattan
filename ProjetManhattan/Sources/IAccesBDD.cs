using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace ProjetManhattan.Sources
{
    public interface IAccesBDD : ISource
    {
        public string ConnectionString { get; init; }
        IDbConnection ConnexionBD();
    }
}

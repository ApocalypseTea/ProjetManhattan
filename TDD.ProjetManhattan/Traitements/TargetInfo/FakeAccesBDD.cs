using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjetManhattan.Sources;
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class FakeAccesBDD : IAccesBDD
    {
        private FakeSqlConnexion _connection;
        public string ConnectionString { get; set; }

        public DataTable ExpectedData { get; set; }

        public IDbConnection ConnexionBD()
        {
            _connection = new FakeSqlConnexion(this);
            return _connection;
        }
    }
}

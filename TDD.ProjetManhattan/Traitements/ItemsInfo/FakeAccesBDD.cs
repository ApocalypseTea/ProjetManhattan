using System.Data;
using Microsoft.Data.Sqlite;
using ProjetManhattan.Sources;
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;

namespace TDD.ProjetManhattan.Traitements.ItemsInfo
{
    public class FakeAccesBDD : IAccesBDD
    {
        public string ConnectionString { get ; set ; }
        public DataTable ExpectedData { get; set; }
        public FakeSqliteConnection _createdConnection;

        public IDbConnection ConnexionBD()
        {
            _createdConnection = new FakeSqliteConnection(this);
            return _createdConnection;
        }
    }
}
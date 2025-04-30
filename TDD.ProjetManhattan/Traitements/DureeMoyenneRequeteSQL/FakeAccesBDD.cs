using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Sources;

namespace TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL
{
    internal class FakeAccesBDD : IAccesBDD
    {
        private bool _hasCreatedInstance;
        private IDbCommand _createdSqlCommand;
        private FakeSqlConnection _createdConnection;

        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDbConnection ConnexionBD()
        {
            _hasCreatedInstance = true;
            _createdConnection = new FakeSqlConnection(this);
            return _createdConnection;
        }

        internal bool HasCreatedInstance()
        {
            return _hasCreatedInstance;
        }

        public IDbCommand CreatedSqlCommand()
        {
            return _createdConnection.CreatedSqlCommand();
        }

        public DataTable ExpectData { get; set; }
    }
}

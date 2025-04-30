using System.Data;
using System.Data.Common;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class SqlFakeCommand : IDbCommand
    {
        private FakeSqlConnexion _connexion;
        private FakeDataParameterCollection _parameters;

        public DataTable DataTable { get; set; }
        public SqlFakeCommand(FakeSqlConnexion connexion)
        {
            _connexion = connexion;
        }

        public string CommandText { get; set; }
        public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CommandType CommandType { get; set; }
        public IDbConnection? Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDataParameterCollection Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = new FakeDataParameterCollection();
                }

                return _parameters;
            }
        }

        public IDbTransaction? Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            return new FakeSqlParameter();
        }

        public void Dispose()
        {

        }

        public int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            this.DataTable = this._connexion._fakeAcces.ExpectedData;
            return new DataTableReader(this.DataTable ?? new DataTable());
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }

        public object? ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public void Prepare()
        {
            throw new NotImplementedException();
        }
    }
}
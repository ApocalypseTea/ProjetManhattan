using System.Data;
using TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL;

namespace TDD.ProjetManhattan.Traitements.ItemsInfo
{
    public class FakeSQLCommand : IDbCommand
    {
        private FakeSqliteConnection _connection;
        public DataTable DataTable { get; set; }
        public FakeSQLCommand(FakeSqliteConnection connection)
        {
            _connection = connection;
        }
        public string CommandText { get ; set; }
        public int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CommandType CommandType { get ; set ; }
        public IDbConnection? Connection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDataParameterCollection Parameters => throw new NotImplementedException();

        public IDbTransaction? Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public IDbDataParameter CreateParameter()
        {
            throw new NotImplementedException();
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
            this.DataTable = this._connection.FakeDatabase.ExpectedData;
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
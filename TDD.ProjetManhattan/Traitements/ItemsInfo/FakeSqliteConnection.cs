using System.Data;
using ProjetManhattan.Sources;
using TDD.ProjetManhattan.Traitements.ItemsInfo;

namespace TDD.ProjetManhattan.Traitements.ItemsInfo
{
    public class FakeSqliteConnection:IDbConnection
    {
        public FakeAccesBDD _fakeAccesBDD;
        public FakeAccesBDD FakeDatabase => _fakeAccesBDD;
        public FakeSQLCommand _createdCommand;

        public FakeSqliteConnection(FakeAccesBDD fakeAccesBDD)
        {
            _fakeAccesBDD = fakeAccesBDD;
        }

        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ConnectionTimeout => throw new NotImplementedException();

        public string Database => throw new NotImplementedException();

        public ConnectionState State => throw new NotImplementedException();

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public IDbCommand CreateCommand()
        {
            _createdCommand = new FakeSQLCommand(this);
            return _createdCommand; 
        }

        public void Dispose()
        {
            
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public FakeSQLCommand CreatedSqlCommand()
        {
            return _createdCommand;
        }

        

    }

}
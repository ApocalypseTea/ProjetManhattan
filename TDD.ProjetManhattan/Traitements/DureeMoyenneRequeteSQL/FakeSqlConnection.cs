using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.ProjetManhattan.Traitements.DureeMoyenneRequeteSQL
{
    internal class FakeSqlConnection : IDbConnection
    {
        private FakeAccesBDD _accesBDD;
        private FakeSqlCommand _createdCommand;

        public FakeSqlConnection(FakeAccesBDD accesBDD)
        {
            _accesBDD = accesBDD;
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
            _createdCommand = new FakeSqlCommand(this);
            return _createdCommand;
        }

        public void Dispose()
        {
            
        }

        public void Open()
        {
            throw new NotImplementedException();
        }

        public FakeSqlCommand CreatedSqlCommand()
        {
            return _createdCommand;
        }

        public FakeAccesBDD FakeDatabase => _accesBDD;
    }
}

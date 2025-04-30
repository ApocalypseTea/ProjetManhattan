using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDD.ProjetManhattan.Traitements.TargetInfo
{
    internal class FakeSqlParameter : IDbDataParameter
    {
        public byte Precision { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public byte Scale { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DbType DbType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ParameterDirection Direction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsNullable => throw new NotImplementedException();

        public string ParameterName { get; set; }
        public string SourceColumn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DataRowVersion SourceVersion { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object? Value { get ; set; }
    }
}

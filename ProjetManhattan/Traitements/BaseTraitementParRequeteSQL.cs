using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    public abstract class BaseTraitementParRequeteSQL <T> : BaseTraitement<IAccesBDD> where T: IToRecordable
    {
        protected List<T> _lines;
        protected BaseTraitementParRequeteSQL(IUnityContainer container) : base(container)
        {
            _lines = new List<T>();
            _source = container.Resolve<IAccesBDD>();
        }
        public override void Execute()
        {
            using (IDbConnection connect = _source.ConnexionBD())
            using (IDbCommand requete = GetSQLCommand(connect))
            using (IDataReader reader = requete.ExecuteReader())
            {
                while(reader.Read())
                {
                    T item = ReadItem(reader);
                    _lines.Add(item);
                    Record line = item.ToRecord();
                    this.AddRecord(line);
                }
            }
        }

        protected abstract T ReadItem(IDataReader reader);       
        protected abstract IDbCommand GetSQLCommand(IDbConnection connection);
        protected string GetSQLQuery(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}

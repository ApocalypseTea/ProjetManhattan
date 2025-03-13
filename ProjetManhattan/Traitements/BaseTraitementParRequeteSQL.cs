using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Sources;

namespace ProjetManhattan.Traitements
{
    abstract class BaseTraitementParRequeteSQL <T> : BaseTraitement<IAccesBDD> where T: IToRecordable
    {
        protected List<T> _items;
        protected BaseTraitementParRequeteSQL(BaseConfig config) : base(config)
        {
            _items = new List<T>();
            _source = new AccesBDD(config);
        }
        public override void Execute()
        {
            using (SqlConnection connect = _source.ConnexionBD())
            using (SqlCommand requete = GetSQLCommand(connect))
            using (SqlDataReader reader = requete.ExecuteReader())
            {
                while(reader.Read())
                {
                    T item = ReadItem(reader);
                    Record line = item.ToRecord();
                    this.AddItem(line);
                }
            }
        }
        protected abstract T ReadItem(SqlDataReader reader);       
        protected abstract SqlCommand GetSQLCommand(SqlConnection connection);
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

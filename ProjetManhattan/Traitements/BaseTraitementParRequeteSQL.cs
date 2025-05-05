using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ProjetManhattan.Configuration;
using ProjetManhattan.Formatages;
using ProjetManhattan.Sources;
using Unity;

namespace ProjetManhattan.Traitements
{
    public abstract class BaseTraitementParRequeteSQL <T> : BaseTraitement<IAccesBDD> where T: IToRecordable
    {
        public List<T> _lines;
        protected BaseTraitementParRequeteSQL(IUnityContainer container) : base(container)
        {
            _lines = new List<T>();
        }
        public override void Execute()
        {
            //Console.WriteLine("execution traitement par requete SQL");
            _source = this.Container.Resolve<IAccesBDD>();
            using (IDbConnection connect = _source.ConnexionBD()) 
            {
                //Console.WriteLine("connexion etablie");
                using (IDbCommand requete = GetSQLCommand(connect))
                {
                    //Console.WriteLine("commande recuperee");
                    using (IDataReader reader = requete.ExecuteReader())
                    {
                        //Console.WriteLine("Lancement reader");
                        while (reader.Read())
                        {
                            //Console.WriteLine("Je suis le reader de resultat de requete SQL a SQL Server");
                            T item = ReadItem(reader);
                            _lines.Add(item);
                            Record[] line = item.ToRecords();
                            this.AddRecord(line);
                        }
                    }
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

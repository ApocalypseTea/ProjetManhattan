using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace ProjetManhattan.Formatages
{
    class RecordToSQLite
    {
        private Record record;

        public RecordToSQLite(Record record)
        {
            this.record = record;
        }

        public void AddRecordToDataBase(SqliteConnection connection)
        {
            string requete = "INSERT INTO record (target, date, value, propertyName, description)" +
                $"VALUES ({this.record.Traitement}, {this.record.Target}, {this.record.Date}, {this.record.Value}, {this.record.PropertyName}, {this.record.Description});"
                ;
            using (SqliteCommand commande = new SqliteCommand(requete, connection))
            {
                commande.ExecuteReader();
            }
        }



    }
}

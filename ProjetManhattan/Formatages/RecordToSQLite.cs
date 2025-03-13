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
            string requete = "INSERT INTO record (traitement, target, date, value, propertyName, description)" +
                $"VALUES (@Traitement, @Target, @Date, @Value, @PropertyName, @Description);"
                ;

            try {
                SqliteCommand commande = new SqliteCommand(requete, connection);

                commande.Parameters.AddWithValue("@Traitement", this.record.Traitement);
                commande.Parameters.AddWithValue("@Target", this.record.Target);
                commande.Parameters.AddWithValue("@Date", this.record.Date.HasValue?this.record.Date:DBNull.Value);
                commande.Parameters.AddWithValue("@Value", this.record.Value);
                commande.Parameters.AddWithValue("@PropertyName", this.record.PropertyName != null ? this.record.PropertyName : DBNull.Value);
                commande.Parameters.AddWithValue("@Description", this.record.Description != null ? this.record.Description : DBNull.Value);

                commande.ExecuteReader();
            }
            catch (SqliteException _sqliteErreur)
            {
                Console.WriteLine(_sqliteErreur.Message);
                throw;
            }
        }
    }
}

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
        private Record _record;
        public RecordToSQLite(Record record)
        {
            this._record = record;
        }

        public void AddRecordToDataBase(SqliteConnection connection)
        {
            string requete = "INSERT INTO record (traitement, target, date, value, propertyName, description)" +
                $"VALUES (@Traitement, @Target, @Date, @Value, @PropertyName, @Description);"
                ;

            try {
                SqliteCommand commande = new SqliteCommand(requete, connection);

                commande.Parameters.AddWithValue("@Traitement", this._record.Traitement);
                commande.Parameters.AddWithValue("@Target", this._record.Target);
                commande.Parameters.AddWithValue("@Date", this._record.Date.HasValue?this._record.Date:DBNull.Value);
                commande.Parameters.AddWithValue("@Value", this._record.Value);
                commande.Parameters.AddWithValue("@PropertyName", this._record.PropertyName != null ? this._record.PropertyName : DBNull.Value);
                commande.Parameters.AddWithValue("@Description", this._record.Description != null ? this._record.Description : DBNull.Value);

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.Data.SqlClient;

namespace ProjetManhattan.Traitements
{
    //
    // Methode d'extension
    //

    // Classe STATIC !!!
    public static class IDbCommandExtension
    {
        // Méthode STATIC !!!
        // Premier paramètre avec "this" 
        public static void AddParameterWithValue(this IDbCommand command, string parameterName, int value)
        {
            IDbDataParameter pParametre = command.CreateParameter();
            pParametre.Value = value;
            pParametre.ParameterName = parameterName;
            command.Parameters.Add(pParametre);
        }

        public static void AddParameterWithValue(this IDbCommand command, string parameterName, DateTime value)
        {
            IDbDataParameter pParametre = command.CreateParameter();
            pParametre.Value = value;
            pParametre.ParameterName = parameterName;
            command.Parameters.Add(pParametre);
        }

        public static SqlParameter AddParameterWithValue(this IDbCommand command, string parameterName, DataTable value)
        {
            SqlParameter pParametre = command.CreateParameter() as SqlParameter;
            pParametre.Value = value;
            pParametre.ParameterName = parameterName;
            command.Parameters.Add(pParametre);
            return pParametre;
        }
    }
}

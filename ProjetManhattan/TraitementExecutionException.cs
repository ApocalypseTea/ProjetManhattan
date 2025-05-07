using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetManhattan
{
    public class TraitementExecutionException : Exception
    {
        public TraitementExecutionException(string message) : base(message) 
        {
            Console.WriteLine();
            Console.WriteLine(message);
        }
        public TraitementExecutionException(string message, Exception innerException) : base(message, innerException) { }

    }
}

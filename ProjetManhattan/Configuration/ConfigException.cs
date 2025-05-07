
namespace ProjetManhattan.Configuration
{
    [Serializable]
    internal class ConfigException : Exception
    {
        public ConfigException()
        {
        }

        public ConfigException(string? message) : base(message)
        {
            //Console.WriteLine();
            //Console.WriteLine(message);
        }

        public ConfigException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
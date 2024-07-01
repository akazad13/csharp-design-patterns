namespace Singleton
{
    // Singleton
    public class Logger
    {
        // Lazy<T> for lazy initialization
        private static readonly Lazy<Logger> _lazyLogger = new(() => new Logger());

        // Instance
        public static Logger Instance
        {
            get
            {
                return _lazyLogger.Value;
                // if (_instance == null)
                // {
                //     _instance = new Logger();
                // }
                // return _instance;
            }
        }

        protected Logger() { }

        // SingletonOperation
        public void Log(string message)
        {
            Console.WriteLine($"Message to log: {message}");
        }
    }
}

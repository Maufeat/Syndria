using System;

namespace SyndriaServer.Utils
{
    public enum LOG_LEVEL
    {
        INFO,
        ERROR,
        DEBUG
    }

    public static class Logger
    {
        private static readonly object WriterLock = new object();

        public static void Write(string msg, LOG_LEVEL level = LOG_LEVEL.INFO)
        {
            lock (WriterLock)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + DateTime.Now.ToString("HH:mm|dd.MM.yyyy") + "] ");
                switch (level)
                {
                    case LOG_LEVEL.INFO:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(msg);
                        break;
                    case LOG_LEVEL.ERROR:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(msg);
                        break;
                    case LOG_LEVEL.DEBUG:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(msg);
                        break;
                    default:
                        break;
                }
                Console.Write("\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}

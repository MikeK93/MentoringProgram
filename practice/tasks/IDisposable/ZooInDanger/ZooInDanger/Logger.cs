using System;

namespace Zoo
{
    public static class Logger
    {
        public static void Log(string str)
        {
            Log(str, string.Empty);
        }

        public static void Log(string str, params object[] args)
        {
            if (AppSettings.IsLoggingEnabled)
                Console.WriteLine(str, args);
        }

        public static void LogYellow(string str)
        {
            LogYellow(str, string.Empty);
        }

        public static void LogYellow(string str, params object[] args)
        {
            if (AppSettings.IsLoggingEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(str, args);
                Console.ResetColor();
            }
        }

        public static void Red(string str, params object[] args)
        {
            if (AppSettings.IsLoggingEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(str, args);
                Console.ResetColor();
            }
        }

        public static void Green(string str)
        {
            if (AppSettings.IsLoggingEnabled)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(str);
                Console.ResetColor();
            }
        }
    }
}
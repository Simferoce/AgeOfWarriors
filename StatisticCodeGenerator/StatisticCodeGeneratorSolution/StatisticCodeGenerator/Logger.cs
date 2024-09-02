using System;
using System.IO;

namespace StatisticCodeGenerator
{
    public static class Logger
    {
        private static string logFilePath;
        private static bool loggingEnabled = true;

        public static void Initialize(string filePath, bool enabled = true)
        {
            logFilePath = filePath;
            loggingEnabled = enabled;

            if (loggingEnabled && !string.IsNullOrEmpty(logFilePath))
            {
                File.WriteAllText(logFilePath, $"Logger initialized at {DateTime.Now}\n");
            }
        }

        public static void Log(string message)
        {
            if (loggingEnabled && !string.IsNullOrEmpty(logFilePath))
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
            }
        }

        public static void SetLoggingEnabled(bool enabled)
        {
            loggingEnabled = enabled;
        }

        public static bool IsLoggingEnabled => loggingEnabled;
    }

}

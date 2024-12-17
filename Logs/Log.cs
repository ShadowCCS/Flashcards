using System;
using System.IO;

namespace FlashcardsMVP.Logs
{
    internal class Log
    {
        // Path to the log directory and file
        private static readonly string resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
        private static readonly string logFilePath = Path.Combine(resourcesPath, "log.txt");

        // Ensure the log directory exists when the application starts
        static Log()
        {
            if (!Directory.Exists(resourcesPath))
            {
                // Create the 'Logs' directory if it doesn't exist
                Directory.CreateDirectory(resourcesPath);
            }
        }

        // Write a normal log entry
        public static void Write(string message)
        {
            string logMessage = FormatLogMessage(message);
            AppendToLog(logMessage);
            Console.WriteLine(logMessage); // Optional: output to console as well
        }

        // Write an error log entry
        public static void Error(string message)
        {
            string logMessage = FormatLogMessage($"ERROR: {message}");
            AppendToLog(logMessage);
            Console.WriteLine(logMessage); // Optional: output to console as well
        }

        // Clean the log file by clearing its contents
        public static void Clean()
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    File.WriteAllText(logFilePath, string.Empty); // Clears the log file
                    Console.WriteLine("Log file cleaned.");
                }
                else
                {
                    Console.WriteLine("Log file does not exist.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning log file: {ex.Message}");
            }
        }

        // Private helper method to format log messages with a timestamp
        private static string FormatLogMessage(string message)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        }

        // Private helper method to append the log message to the log file
        private static void AppendToLog(string message)
        {
            try
            {
                // Append the log message to the file
                File.AppendAllText(logFilePath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Log error if file write fails
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}

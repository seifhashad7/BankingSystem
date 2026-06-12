using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Bank.Model.Entities;

namespace Bank.Model.Logging
{
    public class Logger
    {
        private readonly string _logFilePath;

        public Logger(string logFilePath = "Bank_Activities.txt") => _logFilePath = logFilePath;

        private void WriteToFile(string logLevel, string logMessage)
        {
            var fullMessage = $"[{DateTime.Now:yyyy-MM-dd hh:mm:ss}] [{logLevel}] {logMessage}";

            try
            {
                File.AppendAllText(_logFilePath, fullMessage + Environment.NewLine);
            }
            catch
            {
                //Fallback, Log it to the console
                Console.WriteLine(fullMessage + Environment.NewLine); 
            }
        }

        public void LogInfo(string message)
        {
            WriteToFile("INFO", message);
        }
        public void LogDebug(string message)
        {
            WriteToFile("DEBUG", message);
        }
        public void LogWarning(string message)
        {
            WriteToFile("WARNING", message);
        }
        public void LogError(string error, Exception e = null)
        {
            var errMsg = (e != null) ? $"{error} - Exception: {e.Message}" : error;
            WriteToFile("ERROR", errMsg);
        }

        public void LogTransaction(TransactionType transaction, int accountId, decimal amount)
        {
            WriteToFile("OPERATION", $": {transaction.ToString()} AccountId: {accountId}, Amount: {amount}");
        }
    }
}

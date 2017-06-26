using System;
using System.IO;

namespace TexasHoldem.Loggers
{
    public enum Severity
    {
        Action, //logs to Action Log and prints to the screen
        Warning, //only prints to the screen
        Error, //logs to Error Log and prints to the screen
        Exception //only logs to Error Log
    }

    public class Logger
    {
        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;
        private static readonly object thisLock = new object();
        public static string AppDataPath, ErrorPath, ActionPath;

        private Logger()
        {
        }

        public static void Log(Severity s, string msg)
        {
            lock (thisLock)
            {
                // if 'log' was called from the server project
                AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null
                    ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString()
                    : AppDomain.CurrentDomain.BaseDirectory;
                ErrorPath = AppDataPath + " \\errorLog.txt";
                ActionPath = AppDataPath + "\\actionLog.txt";

                if (msg == "")
                {
                    var exception = new Exception("message is empty.");
                    Log(Severity.Exception, exception.Message);
                    throw exception;
                }

                var action = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;
                var error = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " +
                            s + ": " + msg;
                switch (s)
                {
                    case Severity.Exception:
                        File.AppendAllText(ErrorPath, error + Environment.NewLine);
                        break;
                    case Severity.Error:
                        File.AppendAllText(ErrorPath, error + Environment.NewLine);
                        Console.WriteLine(error);
                        break;
                    case Severity.Action:
                        File.AppendAllText(ActionPath, action + Environment.NewLine);
                        Console.WriteLine(action);
                        break;
                    case Severity.Warning:
                        Console.WriteLine(error);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(s), s, null);
                }
                // Do stuff with file
                // When done we can break loop
            }
        }
    }
}
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
        //private static readonly string app_data_path = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        //public static readonly string errorPath = app_data_path + "\\errorLog.txt";
        //public static readonly string actionPath = app_data_path + "\\actionLog.txt";

        public static readonly string ErrorPath = Directory.GetCurrentDirectory() + "\\errorLog.txt";
        public static readonly string ActionPath = Directory.GetCurrentDirectory() + "\\actionLog.txt";

        private Logger() { }

        public static void Log(Severity s, string msg)
        {
            if (msg == "")
            {
                Log(Severity.Exception, "message is empty.");
                throw new Exception("message is empty.");
            }

            var a = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;
            var e = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " +
                       s + ": " + msg;

            switch (s)
            {
                case Severity.Exception:
                    File.AppendAllText(ErrorPath, e + Environment.NewLine);
                    break;
                case Severity.Error:
                    File.AppendAllText(ErrorPath, e + Environment.NewLine);
                    Console.WriteLine(e);
                    break;
                case Severity.Action:
                    File.AppendAllText(ActionPath, a + Environment.NewLine);
                    Console.WriteLine(e);
                    break;
                case Severity.Warning:
                    Console.WriteLine(e);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(s), s, null);
            }
        }
    }
}
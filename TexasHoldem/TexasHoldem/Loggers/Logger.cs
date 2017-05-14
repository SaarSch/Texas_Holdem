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
      public static string AppDataPath, ErrorPath, ActionPath;

          private Logger() { }

      public static void Log(Severity s, string msg)
      {
          // if 'log' was called from the server project
          AppDataPath = AppDomain.CurrentDomain.GetData("DataDirectory") != null ? AppDomain.CurrentDomain.GetData("DataDirectory").ToString() : AppDomain.CurrentDomain.BaseDirectory;
          ErrorPath = AppDataPath + "\\errorLog.txt";
          ActionPath = AppDataPath + "\\actionLog.txt";

          if (msg == "")
          {
              var exception = new Exception("message is empty.");
              Log(Severity.Exception, exception.Message);
              throw exception;
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
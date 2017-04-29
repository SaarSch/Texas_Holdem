using System;
using System.IO;

public enum Severity
{
    Action, //logs to Action Log and prints to the screen
    Warning, //only prints to the screen
    Error, //logs to Error Log and prints to the screen
    Exception //only logs to Error Log
}

public class Logger
{
    public static readonly string errorPath = Directory.GetCurrentDirectory() + "\\errorLog.txt";
    public static readonly string actionPath = Directory.GetCurrentDirectory() + "\\actionLog.txt";

    private Logger() { }

    public static void Log(Severity s, string msg)
    {
        if (msg == "")
        {
            Log(Severity.Exception, "message is empty.");
            throw new Exception("message is empty.");
        }

        string a = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + msg;
        string e = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " +
            s + ": " + msg;

        switch (s)
        {
            case Severity.Exception:
                File.AppendAllText(errorPath, e + Environment.NewLine);
                break;
            case Severity.Error:
                File.AppendAllText(errorPath, e + Environment.NewLine);
                Console.WriteLine(e);
                break;
            case Severity.Action:
                File.AppendAllText(actionPath, a + Environment.NewLine);
                Console.WriteLine(e);
                break;
            case Severity.Warning:
                Console.WriteLine(e);
                break;
        }
    }
}

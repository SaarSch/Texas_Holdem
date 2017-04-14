using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace AllTests.UnitTests.Loggers
{
    [TestClass]
    public class TestLogger
    {

        [TestMethod]
        public void Log_LogActionAndErrorMessages_LogsGrewLonger()
        {
            long errorLength = 0;
            long actionLength = 0;

            long errorLengthAfter = 0;
            long actionLengthAfter = 0;

            if (System.IO.File.Exists(Logger.errorPath))
            {
                File.WriteAllText(Logger.errorPath, String.Empty);
                errorLength = new System.IO.FileInfo(Logger.errorPath).Length;
            }
            if (System.IO.File.Exists(Logger.actionPath))
            {
                File.WriteAllText(Logger.actionPath, String.Empty);
                actionLength = new System.IO.FileInfo(Logger.actionPath).Length;
            }

            Logger.Log(Severity.Error, "This is an error message.");
            Logger.Log(Severity.Warning, "This is a warning message.");
            Logger.Log(Severity.Action, "This is an action message.");
            Logger.Log(Severity.Exception, "This is an exception message.");

            if (System.IO.File.Exists(Logger.errorPath))
            {
                errorLengthAfter = new System.IO.FileInfo(Logger.errorPath).Length;
            }
            if (System.IO.File.Exists(Logger.actionPath))
            {
                actionLengthAfter = new System.IO.FileInfo(Logger.actionPath).Length;
            }

            Assert.AreEqual(actionLength + 45, actionLengthAfter);
            Assert.AreEqual(errorLength + 110, errorLengthAfter);
        }

        [TestMethod]
        public void Log_LogEmptyErrorMessage_ExceptionThrown()
        {
            try
            {
                Logger.Log(Severity.Error, "");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void Log_LogEmptyActionMessage_ExceptionThrown()
        {
            try
            {
                Logger.Log(Severity.Action, "");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }
    }
}

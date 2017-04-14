using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AllTests.UnitTests.Loggers
{
    [TestClass]
    public class TestLogger
    {
        Logger logger = Logger.Instance;

        [TestMethod]
        public void Log_LogActionAndErrorMessages_LogsGrewLonger()
        {
            long errorLength = 0;
            long actionLength = 0;

            long errorLengthAfter = 0;
            long actionLengthAfter = 0;

            if (System.IO.File.Exists(Logger.errorPath))
            {
                errorLength = new System.IO.FileInfo(Logger.errorPath).Length;
            }
            if (System.IO.File.Exists(Logger.actionPath))
            {
                actionLength = new System.IO.FileInfo(Logger.actionPath).Length;
            }

            logger.Log(Severity.Error, "This is an error message.");
            logger.Log(Severity.Warning, "This is a warning message.");
            logger.Log(Severity.Action, "This is an action message.");
            logger.Log(Severity.Exception, "This is an exception message.");

            if (System.IO.File.Exists(Logger.errorPath))
            {
                errorLengthAfter = new System.IO.FileInfo(Logger.errorPath).Length;
            }
            if (System.IO.File.Exists(Logger.actionPath))
            {
                actionLengthAfter = new System.IO.FileInfo(Logger.actionPath).Length;
            }

            Assert.AreEqual(actionLength + 46, actionLengthAfter);
            Assert.AreEqual(errorLength + 112, errorLengthAfter);
        }

        [TestMethod]
        public void Log_LogEmptyErrorMessage_ExceptionThrown()
        {
            try
            {
                logger.Log(Severity.Error, "");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void Log_LogEmptyActionMessage_ExceptionThrown()
        {
            try
            {
                logger.Log(Severity.Action, "");
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }
    }
}

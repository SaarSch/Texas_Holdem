using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AllTests.UnitTests.Users
{
    [TestClass]
    public class TestUser
    {
        User yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
        User kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
        string notif = "10/04/2017 15:30: This is a notification.";
        string notif2 = "";

        [TestMethod]
        public void AddNotification_AddOneNotification_QueueGotBigger()
        {
            yossi.AddNotification(notif);
            Assert.AreEqual(1, yossi.Notifications.Count);
            yossi.RemoveNotification(notif);
        }

        [TestMethod]
        public void AddNotification_AddEmptyNotification_ExceptionThrown()
        {
            try
            {
                yossi.AddNotification(notif2);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }

        [TestMethod]
        public void RemoveNotification_DeleteOneNotification_QueueGotSmaller()
        {
            kobi.AddNotification(notif);
            kobi.RemoveNotification(notif);
            Assert.AreEqual(0, yossi.Notifications.Count);
        }

        [TestMethod]
        public void RemoveNotification_DeleteEmptyNotification_ExceptionThrown()
        {
            try
            {
                yossi.RemoveNotification(notif2);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }
    }
}

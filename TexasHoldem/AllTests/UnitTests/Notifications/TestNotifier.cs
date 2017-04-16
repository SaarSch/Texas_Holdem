using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace AllTests.UnitTests.Notifications
{
    [TestClass]
    public class TestNotifier
    {
        Notifier notifier = Notifier.Instance;

        [TestMethod]
        public void Notify_NotifyTwoUsers_NotifyAddedToQueues()
        {
            string message = "wow you are so cool!";
            User yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com");
            User kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com");
            List<User> users = new List<User>() { yossi, kobi };

            notifier.Notify(users, message);

            foreach (User u in users)
            {
                Assert.AreEqual(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message,
                    u.Notifications[0]);
            }
        }

        [TestMethod]
        public void Notify_NotifyEmptyNotification_ExceptionThrown()
        {
            string message2 = "";
            User yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com");
            User kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com");
            List<User> users = new List<User>() { yossi, kobi };

            try
            {
                notifier.Notify(users, message2);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception) { }
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem.Notifications;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Notifications
{
    [TestClass]
    public class TestNotifier
    {
        private readonly Notifier notifier = Notifier.Instance;

        [TestMethod]
        public void Notify_NotifyTwoUsers_NotifyAddedToQueues()
        {
            var message = "wow you are so cool!";
            var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            var users = new List<User> {yossi, kobi};

            notifier.Notify(users, message);

            foreach (var u in users)
                Assert.AreEqual(
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message,
                    u.Notifications[0]);
        }

        [TestMethod]
        public void Notify_NotifyEmptyNotification_ExceptionThrown()
        {
            var message2 = "";
            var yossi = new User("KillingHsX", "12345678", "pic.jpg", "hello@gmail.com", 5000);
            var kobi = new User("KillingHsX1", "12345678", "pic1.jpg", "hello@gmail.com", 5000);
            var users = new List<User> {yossi, kobi};

            try
            {
                notifier.Notify(users, message2);
                Assert.Fail(); // If it gets to this line, no exception was thrown
            }
            catch (Exception)
            {
            }
        }
    }
}
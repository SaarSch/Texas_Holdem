using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TexasHoldem.Notifications;
using TexasHoldem.Users;

namespace AllTests.UnitTests.Notifications
{
    [TestClass]
    public class TestNotifier
    {
        private readonly Notifier _notifier = Notifier.Instance;
        private List<IUser> _users;

        [TestInitialize]
        public void Initialize()
        {
            _users = new List<IUser>();
            var mockuser1=new Mock<IUser>();
            var mockuser2 = new Mock<IUser>();
            mockuser1.SetupAllProperties();
            mockuser2.SetupAllProperties();
            var a = mockuser1.Object;
            var b = mockuser2.Object;
            a.Notifications = new List<Tuple<string, string>>();
            b.Notifications = new List<Tuple<string, string>>();
            mockuser1.Setup(r=>r.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string,string>((s1,s2)=>a.Notifications.Add(new Tuple<string, string>(s1, s2)));
            mockuser2.Setup(r => r.AddNotification(It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string>((s1, s2) => b.Notifications.Add(new Tuple<string, string>(s1, s2)));
            _users.Add(a);
            _users.Add(b);
        }

        [TestMethod]
        public void Notify_NotifyTwoUsers_NotifyAddedToQueues()
        {
            const string message = "wow you are so cool!";

            _notifier.Notify(_users,"a", message);

            foreach (var u in _users)
                Assert.AreEqual(
                    DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + ": " + message,
                    u.Notifications[0].Item2);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Notify_NotifyEmptyNotification_ExceptionThrown()
        {
            const string message2 = "";

            _notifier.Notify(_users,"a", message2);
        }
    }
}
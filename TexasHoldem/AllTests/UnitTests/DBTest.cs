using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TexasHoldem;
using TexasHoldem.Users;

namespace AllTests.UnitTests
{
	[TestClass]
	public class DBTest
	{
		private static readonly DatabaseContext db = new DatabaseContext("TestDatabase");
		private static List<User> users;
		[TestInitialize]
		public void Initialize()
		{
			users = new List<User>();
			for (int i = 0; i < 10; i++)
			{
				users.Add(new User("username" + i, "1234456a", "default.png", "deafult@gmail.com", i*1000));
			}
			db.Users.AddRange(users);
			db.SaveChanges();
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (db.Users.Any())
			{
				db.Users.RemoveRange(db.Users);
				db.SaveChanges();
			}
		}
		[TestMethod]
		public void DBAddAndRemove()
		{
			for (int i = 10; i < 20; i++)
			{
				users.Add(new User("username" + i, "1234456a", "default.png", "deafult@gmail.com", 5000));
			}
			db.Users.AddRange(users.GetRange(10,10));
			db.SaveChanges();
			Assert.AreEqual(20, db.Users.Count());
			db.Users.RemoveRange(users);
			db.SaveChanges();
			Assert.AreEqual(0, db.Users.Count());
		}

		[TestMethod]
		public void DBSearchQuery1()
		{
			Assert.AreEqual(1, db.Users.Count(u => u.Username == "username5"));
		}

		[TestMethod]
		public void DBSearchQuery2()
		{
			Assert.AreEqual(10, db.Users.Count(u => u.Password == "1234456a"));
		}

		[TestMethod]
		public void DBModify()
		{
			db.Users.Single(u => u.Username == "username3").Wins = 150;
			db.SaveChanges();
			Assert.AreEqual("username3", db.Users.Single(u => u.Wins == 150).Username);
		}

		[TestMethod]
		public void DBMaxQuery()
		{
			User ans = db.Users.OrderByDescending(u => u.ChipsAmount).First();
			Assert.AreEqual("username9", ans.Username);
		}
	}
}
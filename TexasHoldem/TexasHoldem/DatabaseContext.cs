using System.Data.Entity;
using TexasHoldem.Users;

namespace TexasHoldem
{
	class DatabaseContext : DbContext
	{
		public DatabaseContext(string dbName) 
			: base(dbName) 
		{
		}
		public DbSet<User> Users { get; set; }
	}
}
using System.Data.Entity;
using TexasHoldem.Users;

namespace TexasHoldem
{
	class DatabaseContext : DbContext
	{
		public DatabaseContext() 
			: base("TexasDatabase") 
		{
		}
		public DbSet<User> Users { get; set; }
	}
}
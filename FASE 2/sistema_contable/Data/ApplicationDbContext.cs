using Microsoft.EntityFrameworkCore;
using sistema_contable.models;

namespace sistema_contable.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Document> Documents { get; set; }
		public DbSet<User> Users { get; set; }
	}
}

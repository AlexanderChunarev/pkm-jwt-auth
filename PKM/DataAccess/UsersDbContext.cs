using Microsoft.EntityFrameworkCore;
using PKM.Model;

namespace PKM.DataAccess
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserCredentials> UserCredentials { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserCredentials>(entity => {
                entity.HasIndex(e => e.Login).IsUnique();
            });;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ProductCatalogue.AuthenticationService.Entities;

namespace ProductCatalogue.AuthenticationService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSecret> UserSecrets { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserSecret)           
                .WithOne(us => us.User)  
                .HasForeignKey<UserSecret>(us => us.UserId) 
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

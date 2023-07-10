using Microsoft.EntityFrameworkCore;
using SimpleWebApi.Logic.Helpers;
using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Logic
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = 1, Name = "Admin" });
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@root",
                Salt = "salt-value",
                HashedPassword = HashingHelper.HashPassword("12345678", "salt-value")
            });
            modelBuilder.Entity<UserRoleMapping>().HasData(new UserRoleMapping
            {
                Id = 1,
                UserId = 1,
                UserRoleId = 1
            });
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace SimpleWebApi.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Salt { get; set; }
        public List<UserRoleMapping> Roles { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                       .ValueGeneratedOnAdd();
            builder.Property(x => x.Email)
                       .IsRequired()
                       .HasMaxLength(400);
            builder.Property(x => x.HashedPassword)
                       .IsRequired();
            builder.Property(x => x.Salt)
                       .IsRequired()
                       .HasMaxLength(100);
            builder.HasMany(x => x.Roles);
        }
    }
}

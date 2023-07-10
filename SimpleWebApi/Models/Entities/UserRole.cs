using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace SimpleWebApi.Models.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                        .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                        .IsRequired()
                        .HasMaxLength(100);
        }
    }
}

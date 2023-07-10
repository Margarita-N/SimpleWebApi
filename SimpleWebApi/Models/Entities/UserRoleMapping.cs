using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Security.Principal;

namespace SimpleWebApi.Models.Entities
{
    public class UserRoleMapping
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }

        public UserRole UserRole { get; set; }
    }

    public class UserRoleMappingConfiguration : IEntityTypeConfiguration<UserRoleMapping>
    {
        public void Configure(EntityTypeBuilder<UserRoleMapping> builder)
        {
            builder.Property(x => x.UserId)
                    .IsRequired();
            builder.Property(x => x.UserRoleId)
                    .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebApi.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Account { get; set; }
        public string ReceivingAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    }

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Account).IsRequired();
            builder.Property(x => x.ReceivingAccount).IsRequired();
            builder.Property(x => x.Amount).IsRequired();
        }
    }
}

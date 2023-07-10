using SimpleWebApi.Logic.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SimpleWebApi.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }

        [Required]
        public string? Account { get; set; }

        [Required]
        public string? ReceivingAccount { get; set; }

        [Required]
        [PositiveValue]
        public decimal Amount { get; set; }
    }
}

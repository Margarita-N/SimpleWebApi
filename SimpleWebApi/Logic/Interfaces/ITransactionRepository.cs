using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Logic.Interfaces
{
    public interface ITransactionRepository
    {
        Transaction GetTransaction(int id);
        List<Transaction> GetTransactions();
        Transaction CreateTransaction(Transaction transaction);
    }
}

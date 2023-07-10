using SimpleWebApi.Logic.Interfaces;
using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Logic
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Transaction GetTransaction(int id)
        {
            var transaction = _dbContext.Transactions.Where(x => x.Id == id).FirstOrDefault();

            return transaction;
        }

        public List<Transaction> GetTransactions()
        {
            return _dbContext.Transactions.ToList();
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            try
            {
                var createdTransaction = _dbContext.Transactions.Add(transaction);
                _dbContext.SaveChanges();

                return createdTransaction.Entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}

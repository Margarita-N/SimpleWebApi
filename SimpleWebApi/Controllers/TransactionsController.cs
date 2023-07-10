using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleWebApi.Logic;
using SimpleWebApi.Logic.Helpers;
using SimpleWebApi.Logic.Interfaces;
using SimpleWebApi.Models;
using SimpleWebApi.Models.Entities;

namespace SimpleWebApi.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionsController(ITransactionRepository transactionService)
        {
            _transactionRepository = transactionService;
        }

        [HttpGet("transaction/{id}")]
        public IActionResult GetTransactionById(int id)
        {
            var transaction = _transactionRepository.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            return Ok(new TransactionModel
            {
                Id = transaction.Id,
                Account = transaction.Account,
                ReceivingAccount = transaction.ReceivingAccount,
                Amount = transaction.Amount
            });
        }

        [HttpGet("transactions")]
        public IActionResult GetTransactions()
        {
            var transactions = _transactionRepository.GetTransactions();

            var mappedTransactions = transactions.Select(x => new TransactionModel
            {
                Id = x.Id,
                Account = x.Account,
                ReceivingAccount = x.ReceivingAccount,
                Amount = x.Amount
            });

            return Ok(mappedTransactions);
        }

        [HttpPost("transaction")]
        //[RequireRole(RoleData.AdminDefault)]
        public IActionResult CreateTransaction(TransactionModel transactionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var transaction = new Transaction
            {
                Account = transactionModel.Account,
                ReceivingAccount = transactionModel.ReceivingAccount,
                Amount = transactionModel.Amount
            };

            var createdTransaction = _transactionRepository.CreateTransaction(transaction);

            if (createdTransaction == null)
                return BadRequest();

            var mappedTransaction = new TransactionModel
            {
                Id = createdTransaction.Id,
                Account = createdTransaction.Account,
                ReceivingAccount = createdTransaction.ReceivingAccount,
                Amount = createdTransaction.Amount
            };

            return Ok(mappedTransaction);
        }
    }
}

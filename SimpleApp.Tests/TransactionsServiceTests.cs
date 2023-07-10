using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SimpleWebApi.Controllers;
using SimpleWebApi.Logic;
using SimpleWebApi.Logic.Interfaces;
using SimpleWebApi.Models;
using SimpleWebApi.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleApp.Tests
{
    public class TransactionsServiceTests
    {
        public Mock<ITransactionRepository> _transactionRepositoryMock;
        public TransactionsController _sut;

        public TransactionsServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _sut = new TransactionsController(_transactionRepositoryMock.Object);

            //bypassing authentication since only the functionality of the endpoints is being tested
            var request = new Mock<HttpRequest>();
            request.SetupGet(x => x.HttpContext.User.Identity.IsAuthenticated).Returns(true);

            var context = new Mock<HttpContext>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            _sut.ControllerContext = new ControllerContext
            {
                HttpContext = context.Object
            };
        }

        [Fact]
        public void GetAllTransactions_NoTransactionResponse()
        {
            _transactionRepositoryMock.Setup(x => x.GetTransactions()).Returns(new List<Transaction>());

            var response = _sut.GetTransactions();

            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response);
            var model = Assert.IsAssignableFrom<IEnumerable<TransactionModel>>(actionResult.Value);

            Assert.Empty(model);
        }

        [Fact]
        public void GetAllTransactions_TransactionsExist()
        {

            _transactionRepositoryMock.Setup(x => x.GetTransactions()).Returns(TransactionData());

            var response = _sut.GetTransactions();

            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response);
            var model = Assert.IsAssignableFrom<IEnumerable<TransactionModel>>(actionResult.Value);

            var mappedData = MappedTransactionData();

            Assert.NotNull(model);
            Assert.Equal(mappedData.Count, model.Count());

            for (int i = 0; i < model.Count(); i++)
            {
                Assert.Equal(mappedData[i].Id, model.ElementAt(i).Id);
                Assert.Equal(mappedData[i].Account, model.ElementAt(i).Account);
                Assert.Equal(mappedData[i].ReceivingAccount, model.ElementAt(i).ReceivingAccount);
                Assert.Equal(mappedData[i].Amount, model.ElementAt(i).Amount);
            }
        }

        [Fact]
        public void GetTransactionById_SuccessfulRetrieval()
        {
            _transactionRepositoryMock.Setup(x => x.GetTransaction(1)).Returns(TransactionById());

            var response = _sut.GetTransactionById(1);
            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response);
            var model = Assert.IsAssignableFrom<TransactionModel>(actionResult.Value);

            var mappedData = MappedTransactionById();
            Assert.NotNull(model);

            Assert.Equal(mappedData.Id, model.Id);
            Assert.Equal(mappedData.Account, model.Account);
            Assert.Equal(mappedData.ReceivingAccount, model.ReceivingAccount);
            Assert.Equal(mappedData.Amount, model.Amount);
        }

        [Fact]
        public void GetTransactionById_TransactionNotFound()
        {
            _transactionRepositoryMock.Setup(x => x.GetTransaction(1)).Returns((Transaction)null);

            var response = _sut.GetTransactionById(1);
            var actionResult = Assert.IsAssignableFrom<NotFoundResult>(response);
        }

        [Theory]
        [MemberData(nameof(InvalidTransactionData))]
        public void CreateTransaction_InvalidDataFailure(string account, string receivingAccount, decimal amount)
        {
            _transactionRepositoryMock.Setup(x => x.CreateTransaction(It.Is<Transaction>(x =>
                x.Account == account
                && x.ReceivingAccount == receivingAccount
                && x.Amount == amount)
                )
            )
            .Returns((Transaction)null);

            var response = _sut.CreateTransaction(new TransactionModel
            {
                Account = account,
                ReceivingAccount = receivingAccount,
                Amount = amount
            });

            var actionResult = Assert.IsAssignableFrom<BadRequestResult>(response);
        }

        [Theory]
        [InlineData(new object[] { "12345678", "23456789", 25 })]
        public void CreateTransaction_ValidData_SuccessfulInsert(string account, string receivingAccount, decimal amount)
        {
            _transactionRepositoryMock.Setup(x => x.CreateTransaction(It.Is<Transaction>(x => 
                x.Account==account
                && x.ReceivingAccount==receivingAccount
                && x.Amount==amount)
                )
            )
            .Returns(new Transaction
            {
                Id = 1,
                Account = account,
                ReceivingAccount = receivingAccount,
                Amount = amount
            });

            var response = _sut.CreateTransaction(new TransactionModel
            {
                Account = account,
                ReceivingAccount = receivingAccount,
                Amount = amount
            });

            var actionResult = Assert.IsAssignableFrom<OkObjectResult>(response);
            Assert.NotNull(actionResult.Value);
            var returnObject = Assert.IsAssignableFrom<TransactionModel>(actionResult.Value);
            Assert.NotEqual(default, returnObject.Id);
            Assert.Equal(account, returnObject.Account);
            Assert.Equal(receivingAccount, returnObject.ReceivingAccount);
            Assert.Equal(amount, returnObject.Amount);
        }

        private List<Transaction> TransactionData()
        {
            return new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    Account = "123",
                    ReceivingAccount = "234",
                    Amount = 10
                },
                new Transaction
                {
                    Id = 1,
                    Account = "345",
                    ReceivingAccount = "567",
                    Amount = 150.50m
                }
            };
        }

        private List<TransactionModel> MappedTransactionData()
        {
            return new List<TransactionModel>
            {
                new TransactionModel
                {
                    Id = 1,
                    Account = "123",
                    ReceivingAccount = "234",
                    Amount = 10
                },
                new TransactionModel
                {
                    Id = 1,
                    Account = "345",
                    ReceivingAccount = "567",
                    Amount = 150.50m
                }
            };
        }

        private Transaction TransactionById()
        {
            return new Transaction
            {
                Id = 1,
                Account = "123",
                ReceivingAccount = "234",
                Amount = 10
            };
        }

        private TransactionModel MappedTransactionById()
        {
            return new TransactionModel
            {
                Id = 1,
                Account = "123",
                ReceivingAccount = "234",
                Amount = 10
            };
        }

        public static IEnumerable<object[]> InvalidTransactionData()
        {
            //missing accounts
            yield return new object[] { null, null, 1 };

            //receiving account is null
            yield return new object[] { "12345678", null, 1 };

            //account is null
            yield return new object[] { null, "23456789", 1 };

            //amount is negative
            yield return new object[] { "12345678", "23456789", -10 };
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}

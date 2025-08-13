using MSListsApp.Dapper.Models;
using Microsoft.Data.Sqlite;
using Dapper;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Services.AccountService;
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class AccountTest
    {
        private SqliteConnection _connection;
        private IAccountRepository _repository;
        private IAccountService _service;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);

            _repository = new AccountRepository(_connection);
            _service = new AccountService(_repository);
        }

        [TestMethod]
        public void CreateAccount_ShouldReturnAccountId_AndBeRetrievable()
        {
            // Arrange
            var createDto = new CreateAccountDto
            {
                Avatar = "a.png",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Company = "Test Co",
                AccountStatus = "Active"
            };

            // Act
            var id = _service.CreateAccount(createDto);
            var saved = _service.GetAccountInfoById(id);

            // Assert
            Assert.IsNotNull(saved, "Account should be retrieved after creation.");
            Assert.AreEqual("John", saved.FirstName);
            Assert.AreEqual("Doe", saved.LastName);
            Assert.AreEqual("john@example.com", saved.Email);
            Assert.AreEqual("Active", saved.AccountStatus);
        }

        [TestMethod]
        public void UpdateAccount_ShouldChangeValues()
        {
            // Arrange - tạo account ban đầu
            var id = _service.CreateAccount(new CreateAccountDto
            {
                Avatar = "a.png",
                FirstName = "Old",
                LastName = "Name",
                Email = "old@example.com",
                Company = "Old Co",
                AccountStatus = "Active"
            });

            var updatedDto = new UpdateAccountDto
            {
                Id = id,
                Avatar = "b.png",
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com",
                Company = "New Co",
                AccountStatus = "Inactive"
            };

            // Act - cập nhật
            _service.UpdateAccount(updatedDto);
            var saved = _service.GetAccountInfoById(id);

            // Assert
            Assert.AreEqual("New", saved.FirstName);
            Assert.AreEqual("Inactive", saved.AccountStatus);
            Assert.AreEqual("new@example.com", saved.Email);
        }

        [TestMethod]
        public void DeleteAccount_ShouldRemoveFromDatabase()
        {
            // Arrange - tạo account tạm
            var id = _service.CreateAccount(new CreateAccountDto
            {
                Avatar = "a.png",
                FirstName = "Temp",
                LastName = "User",
                Email = "temp@example.com",
                Company = "Temp Co",
                AccountStatus = "Active"
            });

            // Act - xóa
            _service.DeleteAccount(id);
            var deleted = _service.GetAccountInfoById(id);

            // Assert
            Assert.IsNull(deleted);
        }

    }
}

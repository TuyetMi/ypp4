
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Services.AccountService;
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class AccountTest
    {
        private SqliteConnection _connection = null!;
        private AccountService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);

            var accountRepo = new AccountRepository(_connection);
            _service = new AccountService(accountRepo);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }
        [TestMethod]
        public void GetAccountById_ShouldReturn_CorrectAccount()
        {
            // Arrange
            var dto = new AccountDto
            {
                Avatar = "avatar.png",
                FirstName = "Jane",
                LastName = "Smith",
                DateBirth = new DateTime(1995, 5, 5),
                Email = "jane@example.com",
                Company = "AnotherCompany",
                AccountStatus = "Active",
                AccountPassword = "password123"
            };
            var id = _service.CreateAccount(dto);

            // Act
            var result = _service.GetAccountInfoById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Jane", result!.FirstName);
            Assert.AreEqual("Smith", result.LastName);
            Assert.AreEqual("jane@example.com", result.Email);
            Assert.AreEqual("", result.AccountPassword, "Password should be hidden when returning DTO");
        }






    }
}

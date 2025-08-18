
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
            TestDatabaseHelper.SeedData(_connection);

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
        public void GetAccountInfoById_ValidId_ReturnsCorrectData()
        {
            // Act
            var account = _service.GetAccountInfoById(1);

            // Assert
            Assert.IsNotNull(account);
            Assert.AreEqual("John", account.FirstName);
            Assert.AreEqual("Doe", account.LastName);
            Assert.AreEqual("john@example.com", account.Email);
        }

        [TestMethod]
        public void GetAccountInfoById_IdLessThanOrEqualZero_ThrowsArgumentException()
        {
            var ex = Assert.ThrowsExactly<ArgumentException>(() =>
                _service.GetAccountInfoById(0)
            );
            Console.WriteLine(ex.Message); // sẽ in ra message trong output
            StringAssert.Contains(ex.Message, "Invalid account id.");
        }

        [TestMethod]
        public void GetAccountInfoById_InvalidId_ThrowsKeyNotFoundException()
        {
            var ex = Assert.ThrowsExactly<KeyNotFoundException>(() =>
                _service.GetAccountInfoById(999)
            );
            Console.WriteLine(ex.Message); // sẽ in ra message trong output
            StringAssert.Contains(ex.Message, "not found"); // vẫn assert
        }





    }
}

using MSListsApp.Dapper.Repositories;
using MSListsApp.Dapper.Models;
using Microsoft.Data.Sqlite;
using Dapper;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class AccountTest
    {
        private SqliteConnection _connection;
        private AccountRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            _repository = new AccountRepository(_connection);
            _repository.EnsureTableAccountCreated();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        //Get information of the specified account
        public void GetAccountInfoById_ShouldReturnCorrectUser()
        {
            // Arrange
            var account = new Account
            {
                Avatar = "avatar2.png",
                FirstName = "Jane",
                LastName = "Smith",
                DateBirth = new DateTime(1985, 5, 15),
                Email = "jane.smith@example.com",
                Company = "ExampleCorp",
                AccountStatus = "Active"
            };

            // Thêm dữ liệu qua repo
            var accountId = _repository.Add(account);

            // Act
            var result = _repository.GetAccountInfoById(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(accountId, result.Id);
            Assert.AreEqual(account.FirstName, result.FirstName);
            Assert.AreEqual(account.LastName, result.LastName);
            Assert.AreEqual(account.Avatar, result.Avatar);
            Assert.AreEqual(account.Email, result.Email);
            Assert.AreEqual(account.Company, result.Company);
            Assert.AreEqual(account.AccountStatus, result.AccountStatus);
        }

        [TestMethod]
        //Check if the account update was successful
        public void Update_ShouldModifyAccount()
        {
            // Arrange: Thêm tài khoản mới
            var account = new Account
            {
                Avatar = "avatar_old.png",
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                DateBirth = new DateTime(1990, 1, 1),
                Email = "old.email@example.com",
                Company = "OldCompany",
                AccountStatus = "Inactive"
            };
            var accountId = _repository.Add(account);

            // Thay đổi thông tin tài khoản
            account.Id = accountId;
            account.FirstName = "NewFirstName";
            account.LastName = "NewLastName";
            account.Avatar = "avatar_new.png";
            account.Email = "new.email@example.com";
            account.Company = "NewCompany";
            account.AccountStatus = "Active";

            // Act: Cập nhật tài khoản
            _repository.Update(account);

            // Lấy lại tài khoản
            var updatedAccount = _repository.GetAccountInfoById(accountId);

            // Assert: Kiểm tra thông tin đã được cập nhật đúng
            Assert.IsNotNull(updatedAccount);
            Assert.AreEqual("NewFirstName", updatedAccount.FirstName);
            Assert.AreEqual("NewLastName", updatedAccount.LastName);
            Assert.AreEqual("avatar_new.png", updatedAccount.Avatar);
            Assert.AreEqual("new.email@example.com", updatedAccount.Email);
            Assert.AreEqual("NewCompany", updatedAccount.Company);
            Assert.AreEqual("Active", updatedAccount.AccountStatus);
        }

        [TestMethod]
        public void AddAccount_ShouldReturnNewId_WithConsoleReport()
        {
            // Arrange
            var account = new Account
            {
                Avatar = "avatar.png",
                FirstName = "Alice",
                LastName = "Johnson",
                DateBirth = new DateTime(1995, 6, 20),
                Email = "alice.johnson@example.com",
                Company = "ExampleInc",
                AccountStatus = "Active"
            };

            // Act
            var newId = _repository.Add(account);

            // Assert
            if (newId > 0)
                Console.WriteLine($"Successfully created account with Id = {newId}");
            else
                Console.WriteLine("Failed to create account.");

            Assert.IsTrue(newId > 0, "New account Id should be greater than zero.");
        }

        [TestMethod]
        public void Update_ShouldReturnUpdatedRecord_WithConsoleReport()
        {
            // Arrange
            var account = new Account
            {
                Avatar = "avatar_old.png",
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                DateBirth = new DateTime(1990, 1, 1),
                Email = "old.email@example.com",
                Company = "OldCompany",
                AccountStatus = "Inactive"
            };
            var accountId = _repository.Add(account);

            // Thay đổi 1 trường (ví dụ FirstName)
            account.Id = accountId;
            account.FirstName = "UpdatedName";

            // Act
            _repository.Update(account);

            var updatedAccount = _repository.GetAccountInfoById(accountId);

            // Assert
            Assert.IsNotNull(updatedAccount, "Updated account should not be null.");

            bool isUpdated = updatedAccount.FirstName == "UpdatedName";

            if (isUpdated)
                Console.WriteLine($"Successfully updated account's FirstName to {updatedAccount.FirstName}");
            else
                Console.WriteLine("Failed to update account's FirstName.");

            Assert.IsTrue(isUpdated, "Account FirstName should be updated.");
        }

        [TestMethod]
        public void Delete_ShouldRemoveAccount_WithConsoleReport()
        {
            // Arrange
            var account = new Account
            {
                Avatar = "avatar.png",
                FirstName = "John",
                LastName = "Doe",
                DateBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Company = "ExampleCorp",
                AccountStatus = "Active"
            };
            var accountId = _repository.Add(account);

            // Act
            _repository.Delete(accountId);

            var deletedAccount = _repository.GetAccountInfoById(accountId);

            // Assert và báo kết quả
            if (deletedAccount == null)
                Console.WriteLine($"Successfully deleted account with Id = {accountId}");
            else
                Console.WriteLine($"Failed to delete account with Id = {accountId}");

            Assert.IsNull(deletedAccount, "Account should be null after deletion.");
        }

    }
}

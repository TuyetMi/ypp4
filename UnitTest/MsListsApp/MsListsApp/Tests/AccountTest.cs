using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsListsApp.Models;
using MsListsApp.Services.AccountService;

namespace MsListsApp.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        private AppDbContext? _context;
        private IAccountService? _accountService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // dùng 1 db mới mỗi lần test
                .Options;

            _context = new AppDbContext(options);
            _accountService = new AccountService(_context);
        }

        [TestMethod]
        public async Task AddAccountAsync_Should_Add_Account()
        {
            // Arrange
            var newAccount = new Account
            {
                FirstName = "Tuyết",
                LastName = "Mi",
                Email = "mi@example.com",
                AccountPassword = "abc"
            };

            // Act
            var result = await _accountService.AddAccountAsync(newAccount);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Tuyết", result.FirstName);
            Assert.AreEqual(1, await _context.Accounts.CountAsync());
        }

        [TestMethod]
        public async Task GetAccountByIdAsync_Should_Return_Correct_Account()
        {
            // Arrange
            var account = new Account { FirstName = "Mai", LastName = "Lan", Email = "mai@example.com", AccountPassword = "abc" };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Act
            var fetched = await _accountService.GetAccountByIdAsync(account.Id);

            // Assert
            Assert.IsNotNull(fetched);
            Assert.AreEqual("Lan", fetched.LastName);
        }

        [TestMethod]
        public async Task UpdateAccountAsync_Should_Update_Fields()
        {
            // Arrange
            var account = new Account { FirstName = "Old", LastName = "Name", Email = "old@example.com", AccountPassword = "abc" };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var update = new Account { FirstName = "New", LastName = "Name", Email = "new@example.com" , AccountPassword = "123"};

            // Act
            var result = await _accountService.UpdateAccountAsync(account.Id, update);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New", result.FirstName);
            Assert.AreEqual("new@example.com", result.Email);
        }

        [TestMethod]
        public async Task DeleteAccountAsync_Should_Remove_Account()
        {
            // Arrange
            var account = new Account { FirstName = "User", LastName = "Delete", Email = "delete@example.com", AccountPassword = "abc" };
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            // Act
            var result = await _accountService.DeleteAccountAsync(account.Id);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, await _context.Accounts.CountAsync());
        }
    }
}

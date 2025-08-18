using System.Data;
using MVC.Data;
using MVC.Models;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;

namespace MVC.Tests.AccountTest
{
    [TestClass]
    public class AccountServiceTests
    {
        private IAccountRepository _repository;
        private IAccountService _service;

        [TestInitialize]
        public void Setup()
        {
            TestDatabaseHelper.InitDatabase();
            var connection = TestDatabaseHelper.GetConnection();

            // Tạo repository + service
            _repository = new AccountRepository(connection);
            _service = new AccountService(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            TestDatabaseHelper.CloseDatabase();
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddAccount()
        {
            var account = new Account
            {
                FirstName = "Alice",
                LastName = "Wonderland",
                Email = "alice@test.com",
                Status = AccountStatus.Active
            };

            var id = await _service.CreateAsync(account);
            Assert.IsTrue(id > 0);

            var created = await _service.GetByIdAsync(id);
            Assert.IsNotNull(created);
            Assert.AreEqual("Alice", created!.FirstName);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyAccount()
        {
            var account = new Account
            {
                FirstName = "Bob",
                LastName = "Builder",
                Email = "bob@test.com",
                Status = AccountStatus.Active
            };

            var id = await _service.CreateAsync(account);
            account.Id = id;
            account.FirstName = "BobUpdated";

            await _service.UpdateAsync(account);

            var updated = await _service.GetByIdAsync(id);
            Assert.AreEqual("BobUpdated", updated!.FirstName);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveAccount()
        {
            // Lấy 1 account từ dữ liệu seed
            var accountToDelete = (await _service.GetAllAccountInfoAsync()).First();

            await _service.DeleteAsync(accountToDelete.Id);

            var deleted = await _service.GetByIdAsync(accountToDelete.Id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task GetAllAccountInfoAsync_ShouldReturnAllDtos()
        {
            var dtos = (await _service.GetAllAccountInfoAsync()).ToList();

            Assert.AreEqual(3, dtos.Count); // SeedData có 3 account
            Assert.AreEqual("John", dtos[0].FirstName);
            Assert.AreEqual("Jane", dtos[1].FirstName);
            Assert.AreEqual("Alice", dtos[2].FirstName);
        }

        /* ------------------------------------------- */
        [TestMethod]
        public async Task GetAccountInfoByIdAsync_ShouldReturnCorrectDto()
        {
            // Lấy 1 account từ seed
            var allAccounts = (await _service.GetAllAccountInfoAsync()).ToList();
            var firstAccount = allAccounts.First();

            var dto = await _service.GetAccountInfoByIdAsync(firstAccount.Id);

            Assert.IsNotNull(dto);
            Assert.AreEqual(firstAccount.Id, dto!.Id);
            Assert.AreEqual(firstAccount.FirstName, dto.FirstName);
            Assert.AreEqual(firstAccount.LastName, dto.LastName);
            Assert.AreEqual(firstAccount.Email, dto.Email);
            Assert.AreEqual(firstAccount.Status, dto.Status);
        }



    }
}

using System.Data;
using MVC.Data;
using MVC.Helpers;
using MVC.Models;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;

namespace MVC.Tests.AccountTest
{
    [TestClass]
    public class AccountServiceTests
    {
        private DependencyInjectionConfig _di;
        private DIScope _scope;
        private IAccountService _accountService; 


        [TestInitialize]
        public void Setup()
        {
            // Khởi tạo database test
            TestDatabaseHelper.InitDatabase();

            _di = new DependencyInjectionConfig();

            _di.Register<IDbConnection>(Lifetime.Scoped, scope => TestDatabaseHelper.GetConnection());

            _di.Register<IAccountRepository, AccountRepository>(Lifetime.Scoped);
            _di.Register<IAccountService, AccountService>(Lifetime.Transient);

            // 3. Tạo scope cho test
            _scope = new DIScope(_di);

            // 4. Resolve service trong scope
            _accountService = _scope.Resolve<IAccountService>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose();
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

            var id = await _accountService.CreateAsync(account);
            Assert.IsTrue(id > 0);

            var created = await _accountService.GetByIdAsync(id);
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

            var id = await _accountService.CreateAsync(account);
            account.Id = id;
            account.FirstName = "BobUpdated";

            await _accountService.UpdateAsync(account);

            var updated = await _accountService.GetByIdAsync(id);
            Assert.AreEqual("BobUpdated", updated!.FirstName);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveAccount()
        {

            // Tạo account riêng cho test
            var account = new Account
            {
                FirstName = "Temp",
                LastName = "User",
                Email = "temp@test.com",
                Status = AccountStatus.Active
            };
            var id = await _accountService.CreateAsync(account);

            // Xóa account vừa tạo
            await _accountService.DeleteAsync(id);

            // Kiểm tra
            var deleted = await _accountService.GetByIdAsync(id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task GetAllAccountInfoAsync_ShouldReturnAllDtos()
        {
            var dtos = (await _accountService.GetAllAccountInfoAsync()).ToList();

            Assert.AreEqual(3, dtos.Count); // SeedData có 3 account
            Assert.AreEqual("John", dtos[0].FirstName);
            Assert.AreEqual("Jane", dtos[1].FirstName);
            Assert.AreEqual("Alice", dtos[2].FirstName);
        }

        [TestMethod]
        public async Task GetAccountInfoByIdAsync_ShouldReturnCorrectDto()
        {
            var allAccounts = (await _accountService.GetAllAccountInfoAsync()).ToList();
            var firstAccount = allAccounts.First();

            var dto = await _accountService.GetAccountInfoByIdAsync(firstAccount.Id);

            Assert.IsNotNull(dto);
            Assert.AreEqual(firstAccount.Id, dto!.Id);
            Assert.AreEqual(firstAccount.FirstName, dto.FirstName);
            Assert.AreEqual(firstAccount.LastName, dto.LastName);
            Assert.AreEqual(firstAccount.Email, dto.Email);
            Assert.AreEqual(firstAccount.Status, dto.Status);
        }
    }
}
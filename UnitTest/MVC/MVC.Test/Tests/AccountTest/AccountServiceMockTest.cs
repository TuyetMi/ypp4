using Moq;
using MVC.Dtos.AccountDtos;
using MVC.Models;
using MVC.Repositories.AccountRepository;
using MVC.Services.AccountService;

namespace MVC.Test.Tests.AccountTest
{
    [TestClass]
    public class AccountServiceMockTest
    {
        private Mock<IAccountRepository> _mockRepo = null!;
        private IAccountService _accountService = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IAccountRepository>();
            _accountService = new AccountService(_mockRepo.Object);
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

            _mockRepo.Setup(r => r.CreateAsync(account)).ReturnsAsync(1);

            var id = await _accountService.CreateAsync(account);

            Assert.AreEqual(1, id);
            _mockRepo.Verify(r => r.CreateAsync(account), Times.Once);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldModifyAccount()
        {
            var account = new Account { Id = 1, FirstName = "Bob", Email = "bob@test.com", Status = AccountStatus.Active };

            _mockRepo.Setup(r => r.UpdateAsync(account)).ReturnsAsync(1);
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(account);

            await _accountService.UpdateAsync(account);
            var updated = await _accountService.GetByIdAsync(1);

            Assert.AreEqual("Bob", updated!.FirstName);
            _mockRepo.Verify(r => r.UpdateAsync(account), Times.Once);
        }

        [TestMethod]
        public async Task DeleteAsync_ShouldRemoveAccount()
        {
            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(1);
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Account?)null);

            await _accountService.DeleteAsync(1);
            var deleted = await _accountService.GetByIdAsync(1);

            Assert.IsNull(deleted);
            _mockRepo.Verify(r => r.DeleteAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAccountInfoAsync_ShouldReturnAllDtos()
        {
            var dtos = new List<AccountInfoDto>
    {
        new AccountInfoDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@test.com", Status = AccountStatus.Active },
        new AccountInfoDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@test.com", Status = AccountStatus.Inactive },
        new AccountInfoDto { Id = 3, FirstName = "Alice", LastName = "Wonderland", Email = "alice@test.com", Status = AccountStatus.Active },
    };

            _mockRepo.Setup(r => r.GetAllAccountInfoAsync()).ReturnsAsync(dtos);

            var result = (await _accountService.GetAllAccountInfoAsync()).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("John", result[0].FirstName);
            Assert.AreEqual("Jane", result[1].FirstName);
            Assert.AreEqual("Alice", result[2].FirstName);
        }


        [TestMethod]
        public async Task GetAccountInfoByIdAsync_ShouldReturnCorrectDto()
        {
            var dto = new AccountInfoDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@test.com",
                Status = AccountStatus.Active
            };

            _mockRepo.Setup(r => r.GetAccountInfoByIdAsync(1)).ReturnsAsync(dto);

            var result = await _accountService.GetAccountInfoByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result!.Id);
            Assert.AreEqual("John", result.FirstName);
        }

    }
}

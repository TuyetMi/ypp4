using MVC.Controllers;
using MVC.Helpers;
using MVC.Models;
using MVC.Dtos.AccountDtos;
using System.Text.Json;
using Moq;
using MVC.Services.AccountService;

namespace MVC.Test.AccountTest
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IAccountService> _mockService = null!;
        private AccountController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new Mock<IAccountService>();
            _controller = new AccountController(_mockService.Object);
        }

        [TestMethod]
        public async Task CreateAccount_ShouldReturnNewId()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John", LastName = "Doe" };
            _mockService.Setup(s => s.CreateAsync(account)).ReturnsAsync(1);

            // Act
            var result = await _controller.CreateAccount(account);
            var obj = JsonSerializer.Deserialize<Dictionary<string, int>>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj["Id"]);
        }

        [TestMethod]
        public async Task GetAccountById_ShouldReturnAccount()
        {
            // Arrange
            var account = new Account { Id = 2, FirstName = "Jane", LastName = "Smith" };
            _mockService.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(account);

            // Act
            var result = await _controller.GetAccountById(2);
            var obj = JsonSerializer.Deserialize<Account>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(2, obj.Id);
            Assert.AreEqual("Jane", obj.FirstName);
        }

        [TestMethod]
        public async Task GetAllAccounts_ShouldReturnList()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, FirstName = "John" },
                new Account { Id = 2, FirstName = "Jane" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(accounts);

            // Act
            var result = await _controller.GetAllAccounts();
            var obj = JsonSerializer.Deserialize<List<Account>>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(2, obj.Count);
        }

        [TestMethod]
        public async Task UpdateAccount_ShouldReturnUpdatedRows()
        {
            // Arrange
            var account = new Account { Id = 1, FirstName = "John" };
            _mockService.Setup(s => s.UpdateAsync(account)).ReturnsAsync(1);

            // Act
            var result = await _controller.UpdateAccount(account);
            var obj = JsonSerializer.Deserialize<Dictionary<string, int>>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj["Updated"]);
        }

        [TestMethod]
        public async Task DeleteAccount_ShouldReturnDeletedRows()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(1);

            // Act
            var result = await _controller.DeleteAccount(1);
            var obj = JsonSerializer.Deserialize<Dictionary<string, int>>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj["Deleted"]);
        }

        [TestMethod]
        public async Task GetAccountInfoByIdJson_ShouldReturnDto()
        {
            // Arrange
            var dto = new AccountInfoDto { Id = 5, FirstName = "Alice", LastName = "Wonder" };
            _mockService.Setup(s => s.GetAccountInfoByIdAsync(5)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetAccountInfoById(5);
            var obj = JsonSerializer.Deserialize<AccountInfoDto>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(5, obj.Id);
            Assert.AreEqual("Alice", obj.FirstName);
        }

        [TestMethod]
        public async Task GetAllAccountInfoJson_ShouldReturnDtoList()
        {
            // Arrange
            var list = new List<AccountInfoDto>
            {
                new AccountInfoDto { Id = 1, FirstName = "John" },
                new AccountInfoDto { Id = 2, FirstName = "Jane" }
            };
            _mockService.Setup(s => s.GetAllAccountInfoAsync()).ReturnsAsync(list);

            // Act
            var result = await _controller.GetAllAccountInfo();
            var obj = JsonSerializer.Deserialize<List<AccountInfoDto>>(result);

            // Assert
            Assert.IsNotNull(obj);
            Assert.AreEqual(2, obj.Count);
        }
    }
}

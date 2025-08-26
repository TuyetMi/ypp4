
using MVC.Controllers;
using MVC.Data;
using MVC.Helpers;
using MVC.Models;

namespace MVC.Tests.AccountTest
{
    [TestClass]
    public class AccountControllerTests
    {
        private DIScope _scope;
        private AccountController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Khởi tạo database test
            TestDatabaseHelper.InitDatabase();

            // Tạo DI config và scope
            var di = AppDependencyInjectionConfig.CreateConfig();
            _scope = new DIScope(di);

            // Resolve controller từ DI (controller sẽ lấy service + repository qua DI)
            _controller = _scope.Resolve<AccountController>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _scope.Dispose();                  // Giải phóng scoped services
            TestDatabaseHelper.CloseDatabase(); // Đóng database
        }

        [TestMethod]
        public async Task TestCreateAndGetById()
        {
            var account = new Account { FirstName = "Mi", LastName = "Luong" };

            // Create
            var id = await _controller.CreateAccount(account);
            Assert.IsTrue(id > 0, "Id phải lớn hơn 0");

            // GetById
            var result = await _controller.GetAccountById(id);
            Assert.IsNotNull(result);
            Assert.AreEqual("Mi", result!.FirstName);
            Assert.AreEqual("Luong", result.LastName);
        }

        [TestMethod]
        public async Task TestGetAll()
        {
            var all = await _controller.GetAllAccount();
            Assert.IsTrue(all.Any(), "DB phải có ít nhất một account");
            foreach (var acc in all)
            {
                Assert.IsNotNull(acc.FirstName);
                Assert.IsNotNull(acc.LastName);
            }

            Console.WriteLine($"Có {all.Count()} account trong DB test.");
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            // Lấy account có sẵn
            var account = (await _controller.GetAllAccount()).FirstOrDefault();
            Assert.IsNotNull(account);

            account!.LastName = "Updated";

            var updated = await _controller.UpdateAccount(account);
            Assert.AreEqual(1, updated);

            var result = await _controller.GetAccountById(account.Id);
            Assert.AreEqual("Updated", result!.LastName);
        }

        [TestMethod]
        public async Task TestDelete()
        {
            // Lấy account có sẵn
            var account = (await _controller.GetAllAccount()).FirstOrDefault();
            Assert.IsNotNull(account);

            var deleted = await _controller.DeleteAccount(account!.Id);
            Assert.AreEqual(1, deleted);

            var result = await _controller.GetAccountById(account.Id);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task TestGetSingleAccountDTO()
        {
            // Lấy account có sẵn
            var account = (await _controller.GetAllAccount()).FirstOrDefault();
            Assert.IsNotNull(account);

            // Lấy DTO theo Id
            var dto = await _controller.GetAccountInfoById(account!.Id);
            Assert.IsNotNull(dto);

            // In ra thông tin account
            Console.WriteLine("=== Single Account Info ===");
            Console.WriteLine($"Id: {dto!.Id}");
            Console.WriteLine($"FirstName: {dto.FirstName}");
            Console.WriteLine($"LastName: {dto.LastName}");
            Console.WriteLine("===========================");

            // Kiểm tra dữ liệu DTO
            Assert.AreEqual(account.FirstName, dto.FirstName);
            Assert.AreEqual(account.LastName, dto.LastName);
        }

        [TestMethod]
        public async Task TestGetAllAccountsDTO()
        {
            // Lấy tất cả DTOs
            var allDtos = await _controller.GetAllAccountInfo();
            Assert.IsTrue(allDtos.Any(), "DB phải có ít nhất một account DTO");

            // In ra thông tin tất cả account
            Console.WriteLine("=== All Accounts DTOs ===");
            foreach (var dto in allDtos)
            {
                Console.WriteLine($"Id: {dto.Id}, FirstName: {dto.FirstName}, LastName: {dto.LastName}");
            }
            Console.WriteLine("=========================");

            // Optional: kiểm tra dữ liệu cụ thể có tồn tại
            var firstAccount = allDtos.First();
            Assert.IsNotNull(firstAccount.FirstName);
            Assert.IsNotNull(firstAccount.LastName);
        }
    }
}

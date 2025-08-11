using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using MsListsApp.Models;
using MsListsApp.Services.ListService;
using static System.Net.Mime.MediaTypeNames;
namespace MsListsApp.Tests
{
    [TestClass]
    public class ListServiceTests
    {
        private readonly ListService _service;
        private readonly List<Account> _accounts;
        private readonly List<RecentList> _recentLists;

        public ListServiceTests()
        {
            // Dữ liệu thô để test
            _accounts = new List<Account>
        {
            new Account { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", AccountStatus = "Active", AccountPassword = "pass" },
            new Account { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", AccountStatus = "Active", AccountPassword = "pass" }
        };

            _recentLists = new List<RecentList>
        {
            new RecentList { Id = 1, AccountId = 1, ListId = 101, LastAccessedAt = DateTime.Now.AddDays(-1) },
            new RecentList { Id = 2, AccountId = 1, ListId = 102, LastAccessedAt = DateTime.Now.AddDays(-2) },
            new RecentList { Id = 3, AccountId = 2, ListId = 103, LastAccessedAt = DateTime.Now.AddDays(-3) }
        };

            _service = new ListService(_accounts, _recentLists);
        }

        [TestMethod]
        public async Task GetRecentListsByUserAsync_ValidAccountId_ReturnsRecentLists()
        {
            // Arrange
            int accountId = 1;

            // Act
            var result = await _service.GetRecentListsByUserAsync(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count()); // User 1 có 2 RecentList
            Assert.IsTrue(result.All(rl => rl.AccountId == accountId));
            Assert.IsTrue(result.First().LastAccessedAt > result.Last().LastAccessedAt); // Kiểm tra sắp xếp
        }

        [TestMethod]
        public async Task GetRecentListsByUserAsync_AccountNotFound_ThrowsException()
        {
            // Arrange
            int invalidAccountId = 999;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => _service.GetRecentListsByUserAsync(invalidAccountId));
        }

        [TestMethod]
        public async Task GetRecentListsByUserAsync_NoRecentLists_ReturnsEmptyList()
        {
            // Arrange
            int accountId = 2;
            _recentLists.RemoveAll(rl => rl.AccountId == accountId); // Xóa RecentList của user 2

            // Act
            var result = await _service.GetRecentListsByUserAsync(accountId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count()); // Không có RecentList
        }
    }
}
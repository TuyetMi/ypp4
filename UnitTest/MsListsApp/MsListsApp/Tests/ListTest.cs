using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsListsApp.Models;
using MsListsApp.Services.ListService;

namespace MsListsApp.Tests
{
    [TestClass]
    public class ListServiceTests
    {
        private AppDbContext _context = null!;
        private ListService _listService = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "MsListsDbTest")
                .Options;

            _context = new AppDbContext(options);
            _listService = new ListService(_context);

            // Seed data
            var user = new Account { Id = 1, Email = "user1@test.com" };
            var list1 = new List { Id = 1, ListName = "List A", CreatedBy = 1, ListStatus = "Active", ListTypeId = 1 };
            var list2 = new List { Id = 2, ListName = "List B", CreatedBy = 1, ListStatus = "Active", ListTypeId = 1 };
            var list3 = new List { Id = 3, ListName = "List C", CreatedBy = 1, ListStatus = "Archived", ListTypeId = 1 }; // ❌ Không phải Active


            _context.Accounts.Add(user);
            _context.Lists.AddRange(list1, list2, list3);
            _context.RecentLists.AddRange(
                new RecentList { AccountId = 1, ListId = 1, LastAccessedAt = DateTime.UtcNow.AddMinutes(-1) },
                new RecentList { AccountId = 1, ListId = 2, LastAccessedAt = DateTime.UtcNow.AddMinutes(-2) },
                new RecentList { AccountId = 1, ListId = 3, LastAccessedAt = DateTime.UtcNow.AddMinutes(-3) }
            );
            _context.ListMemberPermissions.AddRange(
                new ListMemberPermission { ListId = 1, AccountId = 1, HighestPermissionId = 1 },
                new ListMemberPermission { ListId = 3, AccountId = 1, HighestPermissionId = 1 }
            // ⚠️ ListId = 2 KHÔNG có permission
            );
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetRecentListsByUser_ShouldReturn_OnlyListsWithPermission_AndActive()
        {
            // Act
            var recentLists = _listService.GetRecentListsByUser(1);

            // Assert
            Assert.AreEqual(1, recentLists.Count); // chỉ còn ListId = 1

            Assert.IsTrue(recentLists.Any(l => l.Id == 1));     // ✅ Có quyền + Active
            Assert.IsFalse(recentLists.Any(l => l.Id == 2));    // ❌ Không có quyền
            Assert.IsFalse(recentLists.Any(l => l.Id == 3));    // ❌ Đã bị Archived
        }
    }
}

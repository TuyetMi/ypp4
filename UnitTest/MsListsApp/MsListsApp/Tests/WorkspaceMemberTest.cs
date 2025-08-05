using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsListsApp.Models;
using MsListsApp.Services.WorkspaceMemberService;

namespace MsListsApp.Tests
{
    [TestClass]
    public class WorkspaceMemberServiceTests
    {
        private AppDbContext _context = null!;
        private WorkspaceMemberService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(options);
            _service = new WorkspaceMemberService(_context);

            // Seed: 1 account + 1 workspace
            _context.Accounts.Add(new Account { Id = 1, FirstName = "Mai", LastName = "Lan", Email = "mai@example.com", AccountPassword = "abc" });
            _context.Workspaces.Add(new Workspace { Id = 1, WorkspaceName = "My Workspace", CreatedAt = DateTime.UtcNow });
            _context.SaveChanges();
        }

        [TestMethod]
        public async Task AddMemberAsync_ShouldAddNewMember()
        {
            var member = new WorkspaceMember
            {
                WorkspaceId = 1,
                AccountId = 1
            };

            var result = await _service.AddMemberAsync(member);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.WorkspaceId);
            Assert.AreEqual(1, result.AccountId);
            Assert.AreEqual("Active", result.MemberStatus);
        }

        [TestMethod]
        public async Task GetMembersByWorkspaceIdAsync_ShouldReturnCorrectMember()
        {
            await _service.AddMemberAsync(new WorkspaceMember { WorkspaceId = 1, AccountId = 1 });

            var members = await _service.GetMembersByWorkspaceIdAsync(1);

            Assert.IsNotNull(members);
            Assert.AreEqual(1, ((System.Collections.ICollection)members).Count);
        }

        [TestMethod]
        public async Task GetMemberAsync_ShouldReturnExactMatch()
        {
            await _service.AddMemberAsync(new WorkspaceMember { WorkspaceId = 1, AccountId = 1 });

            var member = await _service.GetMemberAsync(1, 1);

            Assert.IsNotNull(member);
            Assert.AreEqual(1, member?.WorkspaceId);
            Assert.AreEqual(1, member?.AccountId);
        }

        [TestMethod]
        public async Task RemoveMemberAsync_ShouldSetStatusToRemoved()
        {
            await _service.AddMemberAsync(new WorkspaceMember { WorkspaceId = 1, AccountId = 1 });

            var success = await _service.RemoveMemberAsync(1, 1);

            Assert.IsTrue(success);

            var updated = await _context.WorkspaceMembers
                .FirstOrDefaultAsync(m => m.WorkspaceId == 1 && m.AccountId == 1);

            Assert.AreEqual("Removed", updated?.MemberStatus);
        }

        [TestMethod]
        public async Task UpdateMemberStatusAsync_ShouldChangeStatus()
        {
            await _service.AddMemberAsync(new WorkspaceMember { WorkspaceId = 1, AccountId = 1 });

            var success = await _service.UpdateMemberStatusAsync(1, 1, "Invited");

            Assert.IsTrue(success);

            var member = await _context.WorkspaceMembers
                .FirstOrDefaultAsync(m => m.WorkspaceId == 1 && m.AccountId == 1);

            Assert.AreEqual("Invited", member?.MemberStatus);
        }
    }
}


using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;
using MSListsApp.Dapper.Services.WorkspaceMemberService;


namespace MSListsApp.Dapper.Tests
{
    [TestClass] 
    public class WorkspaceMemberTests
    {
        private SqliteConnection _connection = null!;
        private WorkspaceMemberService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);
            TestDatabaseHelper.SeedData(_connection);

            var wmRepo = new WorkspaceMemberRepository(_connection);
            _service = new WorkspaceMemberService(wmRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Đóng kết nối sau mỗi test
            _connection.Close();
            _connection.Dispose();
        }


        [TestMethod]
        public void GetAccountNamesByWorkspaceId_ShouldReturn_CorrectNames()
        {
            // Workspace 1 có 2 member từ seed: John và Jane
            var names = _service.GetAccountNamesByWorkspaceId(1).ToList();

            Assert.AreEqual(2, names.Count);
            CollectionAssert.Contains(names, "John Doe");
            CollectionAssert.Contains(names, "Jane Smith");
        }

        [TestMethod]
        public void GetWorkspaceNamesByAccountId_ShouldReturn_CorrectWorkspaces()
        {
            // AccountId 2 (Jane) là member của Workspace 1 và Workspace 2
            var names = _service.GetWorkspaceNamesByAccountId(2).ToList();

            Assert.AreEqual(2, names.Count);
            CollectionAssert.Contains(names, "Workspace 1");
            CollectionAssert.Contains(names, "Workspace 2");
        }


    }
}

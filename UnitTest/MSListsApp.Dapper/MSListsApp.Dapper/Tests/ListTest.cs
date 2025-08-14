

using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.ListRepository;
using MSListsApp.Dapper.Services.ListService;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class ListTest
    {
        private SqliteConnection _connection = null!;
        private ListService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);
            TestDatabaseHelper.SeedData(_connection);

            var listRepo = new ListRepository(_connection);
            _service = new ListService(listRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Đóng kết nối sau mỗi test
            _connection.Close();
            _connection.Dispose();
        }
        [TestMethod]
        public void GetListDetail_ShouldReturnCorrectData()
        {
            var list = _service.GetDetailById(1); // gọi service

            Assert.IsNotNull(list);
            Assert.AreEqual("Project Tasks", list.ListName);
            Assert.AreEqual("📋", list.Icon);
            Assert.AreEqual("#4CAF50", list.Color);
            Assert.AreEqual("My List", list.WorkspaceName);
        }

        [TestMethod]
        public void GetListsInPersonalWorkspaceByUser_ShouldReturnCorrectLists()
        {
            // Arrange
            int accountId = 1;

            // Act
            var result = _service.GetListsInPersonalWorkspaceByUser(accountId).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(
                new List<string> { "Project Tasks", "Project Tasks 2" },
                result.Select(l => l.ListName).ToList()
            );
            Assert.IsTrue(result.All(l => l.WorkspaceName == "My List"));
            Assert.IsTrue(result.Any(l => l.IsFavorited));
        }


    }
}

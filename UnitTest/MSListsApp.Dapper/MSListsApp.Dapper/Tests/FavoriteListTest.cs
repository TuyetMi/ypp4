using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.FavoriteListRepository;
using MSListsApp.Dapper.Services.FavoriteListService;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class FavoriteListTest
    {
        private SqliteConnection _connection = null!;
        private FavoriteListService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);
            TestDatabaseHelper.SeedData(_connection);

            var favoriteListRepo = new FavoriteListRepository(_connection);
            _service = new FavoriteListService(favoriteListRepo);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [TestMethod]
        public void GetFavoriteListsByUser_ShouldReturnFavoritedLists()
        {
            // Act
            var lists = _service.GetFavoriteListsByUser(2).ToList();

            // Assert
            Assert.IsTrue(lists.Any(), "Phải trả về ít nhất 1 favorite list");
            Assert.IsTrue(lists.All(l => !string.IsNullOrEmpty(l.ListName)), "ListName không được null");
            Assert.AreEqual("Task List", lists.First().ListName);
        }

        [TestMethod]
        public void GetFavoriteListsByUser_ReturnsCorrectLists_ForAccount1()
        {
            var results = _service.GetFavoriteListsByUser(2).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("Project Tasks", results[0].ListName);
            Assert.AreEqual("Project Tasks 2", results[1].ListName);
        }

        [TestMethod]
        public void GetFavoriteListsByUser_ReturnsCorrectLists_ForAccount2()
        {
            var results = _service.GetFavoriteListsByUser(2).ToList();

            Assert.AreEqual(2, results.Count);
            Assert.AreEqual("Project Tasks", results[0].ListName);
            Assert.AreEqual("Shopping List", results[1].ListName);
        }

    }
}

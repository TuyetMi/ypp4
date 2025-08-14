
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.ListTypeRepository;
using MSListsApp.Dapper.Services.ListTypeService;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class ListTypeTest
    {
        private SqliteConnection _connection = null!;
        private ListTypeService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);
            TestDatabaseHelper.SeedData(_connection);

            var listTypeRepo = new ListTypeRepository(_connection);
            _service = new ListTypeService(listTypeRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Đóng kết nối sau mỗi test
            _connection.Close();
            _connection.Dispose();
        }

        [TestMethod]
        public void GetListTypeById_ShouldReturnCorrectItem()
        {
            // Arrange
            int listTypeId = 1; // giả sử Id này đã tồn tại trong DB

            // Act
            var result = _service.GetListTypeById(listTypeId);

            // Assert
            Assert.AreEqual("List", result.Title);            // thay bằng dữ liệu seed thực tế
            Assert.AreEqual("list.png", result.Icon);
            Assert.AreEqual("A standard list view for tasks or items.", result.ListTypeDescription);
            Assert.AreEqual("list-header.jpg", result.HeaderImage);
        }

        [TestMethod]
        public void GetAllListTypes_ShouldReturnAllItems()
        {
            // Act
            var allItems = _service.GetAllListTypes().ToList();

            // Assert
            Assert.AreEqual(5, allItems.Count);
            Assert.IsTrue(allItems.Any(l => l.Title == "List"));
            Assert.IsTrue(allItems.Any(l => l.Title == "Form"));
        }
    }
}


using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using MSListsApp.Dapper.Services.WorkspaceService;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class WorkspaceTests
    {
        private SqliteConnection _connection = null!;
        private WorkspaceService _service = null!;

        [TestInitialize]
        public void Setup()
        {
            // Tạo database SQLite in-memory
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);

            // Khởi tạo repository + service
            var workspaceRepo = new WorkspaceRepository(_connection);
            _service = new WorkspaceService(workspaceRepo);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [TestMethod]
        public void GetWorkspaceById_ShouldReturn_CorrectWorkspace()
        {
            // Arrange
            var dto = new WorkspaceDto
            {
                WorkspaceName = "Test Workspace",
                CreatedBy = 2,
                IsPersonal = true
            };
            var id = _service.CreateWorkspace(dto);

            // Act
            var result = _service.GetWorkspaceById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Workspace", result!.WorkspaceName);
            Assert.AreEqual(2, result.CreatedBy);
            Assert.IsTrue(result.IsPersonal);
        }
        [TestMethod]
        public void GetWorkspaceNamesByAccountId_ShouldReturn_CorrectNames()
        {
            // Arrange
            var dto1 = new WorkspaceDto { WorkspaceName = "WS1", CreatedBy = 1, IsPersonal = false };
            var dto2 = new WorkspaceDto { WorkspaceName = "WS2", CreatedBy = 1, IsPersonal = false };
            var dto3 = new WorkspaceDto { WorkspaceName = "WS3", CreatedBy = 2, IsPersonal = false };

            _service.CreateWorkspace(dto1);
            _service.CreateWorkspace(dto2);
            _service.CreateWorkspace(dto3);

            // Act
            var names = _service.GetWorkspaceNamesByAccountId(1).ToList();

            // Assert
            Assert.AreEqual(2, names.Count);
            CollectionAssert.Contains(names, "WS1");
            CollectionAssert.Contains(names, "WS2");
            CollectionAssert.DoesNotContain(names, "WS3");
        }
    }

}
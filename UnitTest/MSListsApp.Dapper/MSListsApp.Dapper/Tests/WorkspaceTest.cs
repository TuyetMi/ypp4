
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

    }

}
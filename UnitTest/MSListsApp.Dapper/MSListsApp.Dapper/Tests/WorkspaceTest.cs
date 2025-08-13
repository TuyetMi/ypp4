using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using MSListsApp.Dapper.Services.WorkspaceService;

namespace MSListsApp.Dapper.Tests
{
    [TestClass]
    public class WorkspaceTests
    {
        private SqliteConnection _connection;
        private IWorkspaceRepository _repository;
        private IWorkspaceService _service;

        [TestInitialize]
        public void Setup()
        {
            // Tạo database SQLite in-memory
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);

            // Khởi tạo repository + service
            _repository = new WorkspaceRepository(_connection);
            _service = new WorkspaceService(_repository);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _connection.Dispose();
        }

        [TestMethod]
        public void TestCreateWorkspace()
        {
            var createDto = new WorkspaceCreateDto
            {
                WorkspaceName = "My Workspace",
                CreatedBy = 1,
                IsPersonal = false
            };

            int id = _service.CreateWorkspace(createDto);
            Assert.IsTrue(id > 0, "Workspace ID should be greater than 0");

            var workspace = _service.GetWorkspaceById(id);
            Assert.IsNotNull(workspace);
            Assert.AreEqual("My Workspace", workspace.WorkspaceName);
            Assert.AreEqual(1, workspace.CreatedBy);
            Assert.IsFalse(workspace.IsPersonal);
        }

        [TestMethod]
        public void TestUpdateWorkspace()
        {
            var createDto = new WorkspaceCreateDto
            {
                WorkspaceName = "Initial Workspace",
                CreatedBy = 2,
                IsPersonal = false
            };
            int id = _service.CreateWorkspace(createDto);

            var updateDto = new WorkspaceUpdateDto
            {
                Id = id,
                WorkspaceName = "Updated Workspace",
            };

            _service.UpdateWorkspace(updateDto);

            var updated = _service.GetWorkspaceById(id);
            Assert.AreEqual("Updated Workspace", updated.WorkspaceName);
            Assert.IsTrue(updated.IsPersonal);
            Assert.AreEqual(2, updated.CreatedBy); // CreatedBy vẫn giữ nguyên
        }

        [TestMethod]
        public void TestDeleteWorkspace()
        {
            var createDto = new WorkspaceCreateDto
            {
                WorkspaceName = "To Delete",
                CreatedBy = 3,
                IsPersonal = false
            };
            int id = _service.CreateWorkspace(createDto);

            _service.DeleteWorkspace(id);

            var deleted = _service.GetWorkspaceById(id);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public void TestGetWorkspacesByCreatorId()
        {
            var dto1 = new WorkspaceCreateDto { WorkspaceName = "WS1", CreatedBy = 10 };
            var dto2 = new WorkspaceCreateDto { WorkspaceName = "WS2", CreatedBy = 10 };
            var dto3 = new WorkspaceCreateDto { WorkspaceName = "WS3", CreatedBy = 20 };

            _service.CreateWorkspace(dto1);
            _service.CreateWorkspace(dto2);
            _service.CreateWorkspace(dto3);

            var list = _service.GetWorkspacesByCreatorId(10).ToList();
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.Any(w => w.WorkspaceName == "WS1"));
            Assert.IsTrue(list.Any(w => w.WorkspaceName == "WS2"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;
using MsListsApp.Services.WorkspaceService;

namespace MsListsApp.Tests
{
    [TestClass]
    public class WorkspaceServiceTests
    {
        private IWorkspaceService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new WorkspaceService();
        }

        [TestMethod]
        public async Task CreateWorkspaceAsync_ShouldReturnSameWorkspace()
        {
            // Arrange
            var newWorkspace = new Workspace
            {
                Id = 1,
                WorkspaceName = "Test Workspace",
                CreatedAt = DateTime.UtcNow
            };

            // Act
            var result = await _service.CreateWorkspaceAsync(newWorkspace);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Workspace", result.WorkspaceName);
        }

        [TestMethod]
        public async Task UpdateWorkspaceAsync_IdMismatch_ShouldReturnFalse()
        {
            // Arrange
            var workspace = new Workspace { Id = 1, WorkspaceName = "Old Name" };

            // Act
            var result = await _service.UpdateWorkspaceAsync(999, workspace); // sai id

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task DeleteWorkspaceAsync_ShouldReturnTrue()
        {
            // Act
            var result = await _service.DeleteWorkspaceAsync(1);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task GetAllWorkspacesAsync_ShouldReturnEmptyList()
        {
            var result = await _service.GetAllWorkspacesAsync();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, ((List<Workspace>)result).Count);
        }

        [TestMethod]
        public async Task GetWorkspaceByIdAsync_ShouldReturnNull()
        {
            var result = await _service.GetWorkspaceByIdAsync(123);

            Assert.IsNull(result);
        }
    }
}

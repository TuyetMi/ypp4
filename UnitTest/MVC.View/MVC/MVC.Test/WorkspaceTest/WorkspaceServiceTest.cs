using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MVC.Dtos.WorkspaceDtos;
using MVC.Repositories.WorkspaceRepository;
using MVC.Services.WorkspaceService;

namespace MVC.Test.WorkspaceTest
{
    [TestClass]
    public class WorkspaceServiceTests
    {
        private Mock<IWorkspaceRepository> _repositoryMock;
        private WorkspaceService _service;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IWorkspaceRepository>();
            _service = new WorkspaceService(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task GetById_ReturnsWorkspace()
        {
            var expected = new WorkspaceInfoDto { Id = 1, WorkspaceName = "Test", IsPersonal = false };
            _repositoryMock.Setup(r => r.GetWorkSpaceInfoByIdAsync(1)).ReturnsAsync(expected);

            var result = await _service.GetWorkSpaceInfoByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected.Id, result.Id);
        }

        [TestMethod]
        public async Task GetById_ReturnsNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetWorkSpaceInfoByIdAsync(99)).ReturnsAsync((WorkspaceInfoDto?)null);

            var result = await _service.GetWorkSpaceInfoByIdAsync(99);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetPersonal_ReturnsWorkspace()
        {
            var expected = new WorkspaceInfoDto { Id = 2, WorkspaceName = "My Lists", IsPersonal = true };
            _repositoryMock.Setup(r => r.GetPersonalWorkspaceAsync(123)).ReturnsAsync(expected);

            var result = await _service.GetPersonalWorkspaceAsync(123);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsPersonal);
        }

        [TestMethod]
        public async Task GetPersonal_ReturnsNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetPersonalWorkspaceAsync(456)).ReturnsAsync((WorkspaceInfoDto?)null);

            var result = await _service.GetPersonalWorkspaceAsync(456);

            Assert.IsNull(result);
        }

        // Test DTO

        [TestMethod]
        public async Task GetPersonal_ReturnsMyLists_WhenAccountHasPersonalWorkspace()
        {
            // Arrange
            int accountId = 100;
            var expected = new WorkspaceInfoDto
            {
                Id = 10,
                WorkspaceName = "My Lists",
                IsPersonal = true
            };

            _repositoryMock
                .Setup(r => r.GetPersonalWorkspaceAsync(accountId))
                .ReturnsAsync(expected);

            // Act
            var result = await _service.GetPersonalWorkspaceAsync(accountId);

            // Assert
            Assert.IsNotNull(result, "Personal workspace should not be null");
            Assert.AreEqual("My Lists", result.WorkspaceName);
            Assert.IsTrue(result.IsPersonal);
            _repositoryMock.Verify(r => r.GetPersonalWorkspaceAsync(accountId), Times.Once);
        }

        [TestMethod]
        public async Task GetPersonal_ReturnsNull_WhenAccountHasNoPersonalWorkspace()
        {
            // Arrange
            int accountId = 200;
            _repositoryMock
                .Setup(r => r.GetPersonalWorkspaceAsync(accountId))
                .ReturnsAsync((WorkspaceInfoDto?)null);

            // Act
            var result = await _service.GetPersonalWorkspaceAsync(accountId);

            // Assert
            Assert.IsNull(result, "Should return null when no personal workspace exists");
            _repositoryMock.Verify(r => r.GetPersonalWorkspaceAsync(accountId), Times.Once);
        }


    }
}

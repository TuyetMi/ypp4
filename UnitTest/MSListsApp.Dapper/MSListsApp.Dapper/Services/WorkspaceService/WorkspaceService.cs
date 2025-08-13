
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;

namespace MSListsApp.Dapper.Services.WorkspaceService
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly WorkspaceRepository _repository;

        public WorkspaceService(WorkspaceRepository repository)
        {
            _repository = repository;
        }

        // Tạo mới workspace
        public int CreateWorkspace(WorkspaceDto dto)
        {
            if (string.IsNullOrEmpty(dto.WorkspaceName))
                throw new ArgumentException("WorkspaceName không được để trống.");

            var workspace = new Workspace
            {
                WorkspaceName = dto.WorkspaceName,
                CreatedBy = dto.CreatedBy,
                IsPersonal = dto.IsPersonal,
                CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
                UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow
            };

            return _repository.Add(workspace);
        }

        // Lấy workspace theo Id
        public WorkspaceDto? GetWorkspaceById(int id)
        {
            var workspace = _repository.GetWorkspaceById(id);
            if (workspace == null) return null;

            return new WorkspaceDto
            {
                Id = workspace.Id,
                WorkspaceName = workspace.WorkspaceName,
                CreatedBy = workspace.CreatedBy,
                IsPersonal = workspace.IsPersonal,
                CreatedAt = workspace.CreatedAt,
                UpdatedAt = workspace.UpdatedAt
            };
        }

        // Lấy danh sách tên workspace do account tạo
        public IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId)
        {
            return _repository.GetWorkspaceNamesByAccountId(accountId);
        }



    }
}
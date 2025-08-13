using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;

namespace MSListsApp.Dapper.Services.WorkspaceService
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _workspaceRepository;

        public WorkspaceService(IWorkspaceRepository workspaceRepository)
        {
            _workspaceRepository = workspaceRepository;
        }

        public int CreateWorkspace(WorkspaceCreateDto createDto)
        {
            var workspace = MapToModel(createDto);
            return _workspaceRepository.Add(workspace);
        }

        public void UpdateWorkspace(WorkspaceUpdateDto updateDto)
        {
            var workspace = _workspaceRepository.GetWorkspaceById(updateDto.Id);
            if (workspace == null) throw new Exception("Workspace not found.");

            // Cập nhật các trường nếu được gửi
            if (!string.IsNullOrEmpty(updateDto.WorkspaceName))
                workspace.WorkspaceName = updateDto.WorkspaceName;

            workspace.UpdatedAt = updateDto.UpdatedAt ?? DateTime.Now;

            _workspaceRepository.Update(workspace);
        }

        public void DeleteWorkspace(int workspaceId)
        {
            _workspaceRepository.Delete(workspaceId);
        }

        public WorkspaceDto? GetWorkspaceById(int workspaceId)
        {
            var workspace = _workspaceRepository.GetWorkspaceById(workspaceId);
            return workspace == null ? null : MapToDto(workspace);
        }

        public IEnumerable<WorkspaceDto> GetWorkspacesByCreatorId(int accountId)
        {
            var workspaces = _workspaceRepository.GetWorkspacesByCreatorId(accountId);
            return workspaces.Select(MapToDto);
        }

        // --- Mapping methods nằm trong service ---
        private static WorkspaceDto MapToDto(Workspace workspace)
        {
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

        private static Workspace MapToModel(WorkspaceCreateDto createDto)
        {
            return new Workspace
            {
                WorkspaceName = createDto.WorkspaceName,
                CreatedBy = createDto.CreatedBy,
                IsPersonal = createDto.IsPersonal,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };
        }

    }


}

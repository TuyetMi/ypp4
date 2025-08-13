using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.WorkspaceService
{
    public interface IWorkspaceService
    {
        void DeleteWorkspace(int workspaceId);
        WorkspaceDto? GetWorkspaceById(int workspaceId);
        IEnumerable<WorkspaceDto> GetWorkspacesByCreatorId(int accountId);
        int CreateWorkspace(WorkspaceCreateDto createDto);
        void UpdateWorkspace(WorkspaceUpdateDto updateDto);
    }
}
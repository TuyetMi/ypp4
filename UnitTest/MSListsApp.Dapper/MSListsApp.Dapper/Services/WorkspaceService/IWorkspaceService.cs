using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.WorkspaceService
{
    public interface IWorkspaceService
    {
        int CreateWorkspace(WorkspaceDto dto);
        WorkspaceDto? GetWorkspaceById(int id);
        IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId);
    }
}
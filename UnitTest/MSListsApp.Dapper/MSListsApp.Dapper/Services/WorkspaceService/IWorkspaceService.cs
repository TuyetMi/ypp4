using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.WorkspaceService
{
    public interface IWorkspaceService
    {
        WorkspaceDto? GetWorkspaceById(int id);
        IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId);
    }
}
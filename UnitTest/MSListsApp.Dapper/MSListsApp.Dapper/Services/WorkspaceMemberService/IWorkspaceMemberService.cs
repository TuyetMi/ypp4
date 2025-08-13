using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.WorkspaceMemberService
{
    public interface IWorkspaceMemberService
    {
        int AddMember(WorkspaceMemberDto dto);
        IEnumerable<string> GetAccountNamesByWorkspaceId(int workspaceId);
        WorkspaceMemberDto? GetMemberById(int id);
        IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId);
    }
}
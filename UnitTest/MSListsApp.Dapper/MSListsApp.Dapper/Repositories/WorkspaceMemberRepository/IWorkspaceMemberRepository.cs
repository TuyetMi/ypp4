using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.WorkspaceMemberRepository
{
    public interface IWorkspaceMemberRepository
    {
        int Add(WorkspaceMember member);
        void CreateTable();
        IEnumerable<string> GetAccountNamesByWorkspaceId(int workspaceId);
        WorkspaceMember? GetById(int id);
        IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId);
    }
}
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.WorkspaceMemberRepository
{
    public interface IWorkspaceMemberRepository
    {
        int Add(WorkspaceMember workspaceMember);
        void Delete(int id);
        void EnsureTableWorkspaceMemberCreated();
        WorkspaceMemberDto? GetById(int id);
        void Update(WorkspaceMemberDto workspaceMember);
    }
}
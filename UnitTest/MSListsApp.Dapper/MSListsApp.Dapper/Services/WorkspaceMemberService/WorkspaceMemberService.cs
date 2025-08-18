
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;

namespace MSListsApp.Dapper.Services.WorkspaceMemberService
{
    public class WorkspaceMemberService : IWorkspaceMemberService
    {
        private readonly IWorkspaceMemberRepository _repository;

        public WorkspaceMemberService(IWorkspaceMemberRepository repository)
        {
            _repository = repository;
        }

        // Lấy member theo Id
        public WorkspaceMemberDto? GetMemberById(int id)
        {
            var member = _repository.GetById(id);
            if (member == null) return null;

            return new WorkspaceMemberDto
            {
                Id = member.Id,
                WorkspaceId = member.WorkspaceId,
                AccountId = member.AccountId,
                JoinedAt = member.JoinedAt,
                MemberStatus = member.MemberStatus,
                UpdatedAt = member.UpdatedAt
            };
        }

        // Lấy tên các account là member của workspace
        public IEnumerable<string> GetAccountNamesByWorkspaceId(int workspaceId)
        {
            return _repository.GetAccountNamesByWorkspaceId(workspaceId);
        }

        // Lấy tên các workspace mà account là member
        public IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId)
        {
            return _repository.GetWorkspaceNamesByAccountId(accountId);
        }

    }

}

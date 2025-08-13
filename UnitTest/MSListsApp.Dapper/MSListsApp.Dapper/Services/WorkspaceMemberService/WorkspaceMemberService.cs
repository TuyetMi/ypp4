
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

        public int AddMember(WorkspaceMemberDto dto)
        {
            if (dto.WorkspaceId <= 0)
                throw new ArgumentException("WorkspaceId phải lớn hơn 0.");
            if (dto.AccountId <= 0)
                throw new ArgumentException("AccountId phải lớn hơn 0.");

            var member = new WorkspaceMember
            {
                WorkspaceId = dto.WorkspaceId,
                AccountId = dto.AccountId,
                JoinedAt = dto.JoinedAt ?? DateTime.UtcNow,
                MemberStatus = dto.MemberStatus,
                UpdatedAt = dto.UpdatedAt ?? DateTime.UtcNow
            };

            return _repository.Add(member);
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

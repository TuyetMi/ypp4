
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

        public int AddMember(WorkspaceMemberDto createDto)
        {
            var member = new WorkspaceMember
            {
                WorkspaceId = createDto.WorkspaceId,
                AccountId = createDto.AccountId,
                JoinedAt = createDto.JoinedAt ?? DateTime.Now,
                MemberStatus = createDto.MemberStatus,
                UpdatedAt = createDto.UpdatedAt
            };
            return _repository.Add(member);
        }

        public void UpdateMember(WorkspaceMemberDto updateDto)
        {
            var member = _repository.GetById(updateDto.Id);
            if (member == null) throw new Exception("Member not found.");

            // Update từng trường nếu có
            if (updateDto.WorkspaceId != 0) member.WorkspaceId = updateDto.WorkspaceId;
            if (updateDto.AccountId != 0) member.AccountId = updateDto.AccountId;
            member.JoinedAt = updateDto.JoinedAt ?? member.JoinedAt;
            member.MemberStatus = updateDto.MemberStatus ?? member.MemberStatus;
            member.UpdatedAt = updateDto.UpdatedAt ?? DateTime.Now;

            _repository.Update(member);
        }

        public void DeleteMember(int memberId)
        {
            _repository.Delete(memberId);
        }


    }

}

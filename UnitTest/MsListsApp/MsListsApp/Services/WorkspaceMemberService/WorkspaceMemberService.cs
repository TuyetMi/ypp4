using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.WorkspaceMemberService
{
    public class WorkspaceMemberService : IWorkspaceMemberService
    {
        // Tạm dùng list để giả lập DB (nếu có DbContext mình thay sau)
        private readonly List<WorkspaceMember> _members = new();
        private AppDbContext context;

        public WorkspaceMemberService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<WorkspaceMember>> GetMembersByWorkspaceIdAsync(int workspaceId)
        {
            await Task.CompletedTask;
            return _members.Where(m => m.WorkspaceId == workspaceId && m.MemberStatus == "Active");
        }

        public async Task<WorkspaceMember?> GetMemberAsync(int workspaceId, int accountId)
        {
            await Task.CompletedTask;
            return _members.FirstOrDefault(m => m.WorkspaceId == workspaceId && m.AccountId == accountId);
        }

        public async Task<WorkspaceMember> AddMemberAsync(WorkspaceMember member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            member.JoinedAt = DateTime.UtcNow;
            member.MemberStatus = "Active";
            member.Id = _members.Count + 1;

            _members.Add(member);
            await Task.CompletedTask;
            return member;
        }

        public async Task<bool> RemoveMemberAsync(int workspaceId, int accountId)
        {
            var member = _members.FirstOrDefault(m => m.WorkspaceId == workspaceId && m.AccountId == accountId);
            if (member == null) return false;

            member.MemberStatus = "Removed";
            member.UpdatedAt = DateTime.UtcNow;

            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> UpdateMemberStatusAsync(int workspaceId, int accountId, string newStatus)
        {
            var member = _members.FirstOrDefault(m => m.WorkspaceId == workspaceId && m.AccountId == accountId);
            if (member == null) return false;

            member.MemberStatus = newStatus;
            member.UpdatedAt = DateTime.UtcNow;

            await Task.CompletedTask;
            return true;
        }
    }
}

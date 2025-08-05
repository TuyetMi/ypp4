using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.WorkspaceMemberService
{
    public interface IWorkspaceMemberService
    {
        Task<IEnumerable<WorkspaceMember>> GetMembersByWorkspaceIdAsync(int workspaceId);
        Task<WorkspaceMember?> GetMemberAsync(int workspaceId, int accountId);
        Task<WorkspaceMember> AddMemberAsync(WorkspaceMember member);
        Task<bool> RemoveMemberAsync(int workspaceId, int accountId);
        Task<bool> UpdateMemberStatusAsync(int workspaceId, int accountId, string newStatus);
    }
}

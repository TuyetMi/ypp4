using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.WorkspaceService
{
    public interface IWorkspaceService
    {
        Task<IEnumerable<Workspace>> GetAllWorkspacesAsync();
        Task<Workspace?> GetWorkspaceByIdAsync(int id);
        Task<Workspace> CreateWorkspaceAsync(Workspace workspace);
        Task<bool> UpdateWorkspaceAsync(int id, Workspace workspace);
        Task<bool> DeleteWorkspaceAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.WorkspaceService
{
    public class WorkspaceService() : IWorkspaceService
    {
        public async Task<IEnumerable<Workspace>> GetAllWorkspacesAsync()
        {
            await Task.CompletedTask;
            return new List<Workspace>();
        }

        public async Task<Workspace?> GetWorkspaceByIdAsync(int id)
        {
            await Task.CompletedTask;
            return null;
        }

        public async Task<Workspace> CreateWorkspaceAsync(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException(nameof(workspace));

            await Task.CompletedTask;
            return workspace;
        }

        public async Task<bool> UpdateWorkspaceAsync(int id, Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException(nameof(workspace));

            if (id != workspace.Id)
                return false;

            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> DeleteWorkspaceAsync(int id)
        {
            await Task.CompletedTask;
            return true;
        }
    }
}

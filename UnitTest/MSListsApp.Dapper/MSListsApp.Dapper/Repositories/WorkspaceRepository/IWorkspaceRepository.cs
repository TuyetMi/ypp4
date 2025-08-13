using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.WorkspaceRepository
{
    public interface IWorkspaceRepository
    {
        void EnsureTableWorkspaceCreated();
        int Add(Workspace workspace);
        void Update(Workspace workspace);
        void Delete(int id);
        Workspace? GetWorkspaceById(int workspaceId);
        IEnumerable<Workspace> GetWorkspacesByCreatorId(int accountId);
    }
}

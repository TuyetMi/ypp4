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
        int Add(Workspace workspace);
        void EnsureTableWorkspaceCreated();
        Workspace GetWorkspaceById(int id);
        IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId);
    }
}

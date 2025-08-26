using MVC.Dtos.WorkspaceDtos;
using MVC.Models;

namespace MVC.Services.WorkspaceService
{
    public interface IWorkspaceService
    {
        Task<int> CreateAsync(Workspace workspace);
        Task<int> DeleteAsync(int id);
        Task<WorkspaceInfoDto?> GetWorkSpaceInfoByIdAsync(int id);
        Task<WorkspaceInfoDto?> GetPersonalWorkspaceAsync(int accountId);
        Task<int> UpdateAsync(Workspace workspace);
    }
}
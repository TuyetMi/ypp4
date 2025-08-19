using MVC.Dtos.WorkspaceDtos;
using MVC.Models;

namespace MVC.Repositories.WorkspaceRepository
{
    public interface IWorkspaceRepository
    {
        Task<int> CreateAsync(Workspace workspace);      // Tạo mới
        Task<Workspace?> GetByIdAsync(int id);        // Lấy theo Id
        Task<IEnumerable<Workspace>> GetAllAsync();   // Lấy tất cả
        Task<int> UpdateAsync(Workspace workspace);     // Cập nhật
        Task<int> DeleteAsync(int id);
        Task<WorkspaceInfoDto?> GetWorkSpaceInfoByIdAsync(int id);
        Task<WorkspaceInfoDto?> GetPersonalWorkspaceAsync(int createdBy);
    }
}
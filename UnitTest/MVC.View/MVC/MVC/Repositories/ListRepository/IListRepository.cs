using MVC.Dtos.ListDtos;
using MVC.Models;

namespace MVC.Repositories.ListRepository
{
    public interface IListRepository
    {
        Task<int> CreateAsync(List list);      // Tạo mới
        Task<List?> GetByIdAsync(int id);        // Lấy theo Id
        Task<IEnumerable<List>> GetAllAsync();   // Lấy tất cả
        Task<int> UpdateAsync(List list);     // Cập nhật
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<ListInfoDto>> GetRecentListsByUserAsync(int accountId);
    }
}
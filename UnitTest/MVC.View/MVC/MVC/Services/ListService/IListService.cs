using MVC.Dtos.ListDtos;
using MVC.Models;

namespace MVC.Services.ListService
{
    internal interface IListService
    {
        Task<int> CreateAsync(List list);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<List>> GetAllAsync();
        Task<List?> GetByIdAsync(int id);
        Task<IEnumerable<ListInfoDto>> GetFavoriteListsAsync(int accountId);
        Task<IEnumerable<ListInfoDto>> GetRecentListsAsync(int accountId);
        Task<int> UpdateAsync(List list);
    }
}
using MVC.Dtos.ListDtos;
using MVC.Models;
using MVC.Repositories.ListRepository;

namespace MVC.Services.ListService
{
    internal class ListService : IListService
    {
        private readonly IListRepository _repository;

        public ListService(IListRepository repository)
        {
            _repository = repository;
        }

        // CRUD
        public Task<int> CreateAsync(List list) => _repository.CreateAsync(list);
        public Task<List?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<IEnumerable<List>> GetAllAsync() => _repository.GetAllAsync();
        public Task<int> UpdateAsync(List list) => _repository.UpdateAsync(list);
        public Task<int> DeleteAsync(int id) => _repository.DeleteAsync(id);

        public async Task<IEnumerable<ListInfoDto>> GetRecentListsAsync(int accountId)
        {
            // Có thể thêm logic xử lý business ở đây, ví dụ: filter, mapping, logging,...
            var lists = await _repository.GetRecentListsByAccountAsync(accountId);
            return lists;
        }

        public async Task<IEnumerable<ListInfoDto>> GetFavoriteListsAsync(int accountId)
        {
            var lists = await _repository.GetFavoritesListByAccountAsync(accountId);
            return lists;
        }
    }
}

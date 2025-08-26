

namespace MVC.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> CreateAsync(T entity);
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
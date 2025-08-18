using MVC.Dtos.AccountDtos;
using MVC.Models;

namespace MVC.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        Task<int> CreateAsync(Account account);      // Tạo mới
        Task<Account?> GetByIdAsync(int id);        // Lấy theo Id
        Task<IEnumerable<Account>> GetAllAsync();   // Lấy tất cả
        Task<int> UpdateAsync(Account account);     // Cập nhật
        Task<int> DeleteAsync(int id);              // Xóa

        Task<AccountInfoDto?> GetAccountInfoByIdAsync(int id);
        Task<IEnumerable<AccountInfoDto>> GetAllAccountInfoAsync();
    }
}
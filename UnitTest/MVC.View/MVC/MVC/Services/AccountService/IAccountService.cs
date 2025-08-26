using MVC.Dtos.AccountDtos;
using MVC.Models;

namespace MVC.Services.AccountService
{
    public interface IAccountService
    {
        Task<int> CreateAsync(Account account);
        Task<int> DeleteAsync(int id);
        Task<AccountInfoDto?> GetAccountInfoByIdAsync(int id);
        Task<IEnumerable<AccountInfoDto>> GetAllAccountInfoAsync();
        Task<IEnumerable<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(int id);
        Task<int> UpdateAsync(Account account);
    }
}
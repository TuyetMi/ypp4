using MVC.Models;
using MVC.Dtos.AccountDtos;
using MVC.Services.AccountService;

namespace MVC.Controllers
{
    public class AccountController
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        // ===== CRUD =====
        public async Task<int> CreateAccount(Account account)
        {
            return await _service.CreateAsync(account);
        }

        public async Task<Account?> GetAccountById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Account>> GetAllAccount()
        {
            return await _service.GetAllAsync();
        }

        public async Task<int> UpdateAccount(Account account)
        {
            return await _service.UpdateAsync(account);
        }

        public async Task<int> DeleteAccount(int id)
        {
            return await _service.DeleteAsync(id);
        }

        // ===== DTO =====
        public async Task<AccountInfoDto?> GetAccountInfoById(int id)
        {
            return await _service.GetAccountInfoByIdAsync(id);
        }

        public async Task<IEnumerable<AccountInfoDto>> GetAllAccountInfo()
        {
            return await _service.GetAllAccountInfoAsync();
        }
    }
}

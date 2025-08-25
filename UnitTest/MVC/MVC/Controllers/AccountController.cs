using MVC.Models;
using MVC.Dtos.AccountDtos;
using MVC.Services.AccountService;
using MVC.Server; // để dùng HttpGet/HttpPost...

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
        [HttpPost]
        public async Task<int> CreateAccount(Account account)
        {
            return await _service.CreateAsync(account);
        }

        [HttpGet]
        public async Task<Account?> GetAccountById(int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Account>> GetAllAccount()
        {
            return await _service.GetAllAsync();
        }

        [HttpPut]
        public async Task<int> UpdateAccount(Account account)
        {
            return await _service.UpdateAsync(account);
        }

        [HttpDelete]
        public async Task<int> DeleteAccount(int id)
        {
            return await _service.DeleteAsync(id);
        }

        // ===== DTO =====
        [HttpGet]
        public async Task<AccountInfoDto?> GetAccountInfoById(int id)
        {
            return await _service.GetAccountInfoByIdAsync(id);
        }

        [HttpGet]
        public async Task<IEnumerable<AccountInfoDto>> GetAllAccountInfo()
        {
            return await _service.GetAllAccountInfoAsync();
        }
    }
}

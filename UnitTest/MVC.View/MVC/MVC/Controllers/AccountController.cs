
using MVC.Models;
using MVC.Services.AccountService;
using System.Text.Json; 

namespace MVC.Controllers
{
    public class AccountController
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        // ===== CRUD API =====

        public async Task<string> CreateAccount(Account account)
        {
            var id = await _service.CreateAsync(account);
            return JsonSerializer.Serialize(new { Id = id });
        }

        public async Task<string> GetAccountById(int id)
        {
            var account = await _service.GetByIdAsync(id);
            return JsonSerializer.Serialize(account);
        }

        public async Task<string> GetAllAccounts()
        {
            var accounts = await _service.GetAllAsync();
            return JsonSerializer.Serialize(accounts);
        }

        public async Task<string> UpdateAccount(Account account)
        {
            var rows = await _service.UpdateAsync(account);
            return JsonSerializer.Serialize(new { Updated = rows });
        }

        public async Task<string> DeleteAccount(int id)
        {
            var rows = await _service.DeleteAsync(id);
            return JsonSerializer.Serialize(new { Deleted = rows });
        }

        // ===== DTO API =====

        public async Task<string> GetAccountInfoById(int id)
        {
            var account = await _service.GetAccountInfoByIdAsync(id);

            if (account == null)
            {
                // Trả JSON rõ ràng hơn thay vì {}
                return JsonSerializer.Serialize(new { error = "Account not found" });
            }
            // Convert object -> JSON string
            return JsonSerializer.Serialize(account);
        }
        public async Task<string> GetAllAccountInfo()
        {
            var list = await _service.GetAllAccountInfoAsync();
            return JsonSerializer.Serialize(list);
        }
    }
}

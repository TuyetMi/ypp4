using MVC.Models;
using MVC.Dtos.AccountDtos;
using MVC.Services.AccountService;

using System.Text.Json; // để dùng HttpGet/HttpPost...

namespace MVC.Controllers
{
    public class AccountController
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        public async Task<string> GetAccountInfoByIdJson(int id)
        {
            var account = await _service.GetAccountInfoByIdAsync(id);

            if (account == null)
                return "{}"; // JSON rỗng nếu không tìm thấy

            // Convert object -> JSON string
            return JsonSerializer.Serialize(account);
        }
    }
}

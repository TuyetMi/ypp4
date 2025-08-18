

namespace MVC.Controllers
{
    public class AccountController
    { }
}
//        private readonly IAccountService _service;

//        public AccountController(IAccountService service)
//        {
//            _service = service;
//        }


//        public async Task ShowAccountAsync(int id)
//        {
//            var account = await _service.GetAccountInfoByIdAsync(id);
//            if (account == null)
//            {
//                Console.WriteLine($"Account with id {id} not found.");
//                return;
//            }

//            Console.WriteLine("=== Account Info ===");
//            Console.WriteLine($"ID: {account.Id}");
//            Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
//            Console.WriteLine($"Email: {account.Email}");
//            Console.WriteLine($"Company: {account.Company}");
//            Console.WriteLine($"Status: {account.Status}");
//        }

//        public async Task CreateAccountAsync(Account account)
//        {
//            var id = await _service.CreateAsync(account);
//            Console.WriteLine($"Created new account with Id = {id}");
//        }

//        // Cập nhật account
//        public async Task UpdateAccountAsync(Account account)
//        {
//            await _service.UpdateAsync(account);
//            Console.WriteLine($" Updated account with Id = {account.Id}");
//        }

//        // Xóa account
//        public async Task DeleteAccountAsync(int id)
//        {
//            await _service.DeleteAsync(id);
//            Console.WriteLine($" Deleted account with Id = {id}");
//        }

//    }
//}

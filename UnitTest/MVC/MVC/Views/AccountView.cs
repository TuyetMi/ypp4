
//using MVC.Controllers;
//using MVC.Models;

//namespace MVC.Views
//{
//    public class AccountView
//    {
//        private readonly AccountController  _controller;

//        public AccountView(AccountController controller)
//        {
//            _controller = controller;
//        }

//        public async Task ShowMenuAsync()
//        {
//            while (true)
//            {
//                Console.WriteLine("\n=== Account Management ===");
//                Console.WriteLine("1. Create Account");
//                Console.WriteLine("2. Show Account by Id");
//                Console.WriteLine("3. Update Account");
//                Console.WriteLine("4. Delete Account");
//                Console.WriteLine("0. Exit");
//                Console.Write("Choose: ");

//                var choice = Console.ReadLine();

//                switch (choice)
//                {
//                    case "1":
//                        await CreateAccountAsync();
//                        break;
//                    case "2":
//                        await ShowAccountAsync();
//                        break;
//                    case "3":
//                        await UpdateAccountAsync();
//                        break;
//                    case "4":
//                        await DeleteAccountAsync();
//                        break;
//                    case "0":
//                        return;
//                    default:
//                        Console.WriteLine("❌ Invalid choice.");
//                        break;
//                }
//            }
//        }

//        private async Task CreateAccountAsync()
//        {
//            var acc = new Account();

//            Console.Write("First Name: ");
//            acc.FirstName = Console.ReadLine();

//            Console.Write("Last Name: ");
//            acc.LastName = Console.ReadLine();

//            Console.Write("Email: ");
//            acc.Email = Console.ReadLine();

//            Console.Write("Company: ");
//            acc.Company = Console.ReadLine();

//            Console.Write("Date of Birth (yyyy-mm-dd): ");
//            acc.DateBirth = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");

//            Console.Write("Avatar path: ");
//            acc.Avatar = Console.ReadLine();

//            Console.Write("Status: ");
//            acc.AccountStatus = Console.ReadLine();

//            Console.Write("Password: ");
//            acc.AccountPassword = Console.ReadLine();

//            await _controller.CreateAccountAsync(acc);
//        }

//        private async Task ShowAccountAsync()
//        {
//            Console.Write("Enter Account Id: ");
//            var id = int.Parse(Console.ReadLine() ?? "0");
//            await _controller.ShowAccountAsync(id);
//        }

//        private async Task UpdateAccountAsync()
//        {
//            Console.Write("Enter Account Id to update: ");
//            var id = int.Parse(Console.ReadLine() ?? "0");

//            var acc = new Account { Id = id };

//            Console.Write("First Name: ");
//            acc.FirstName = Console.ReadLine();

//            Console.Write("Last Name: ");
//            acc.LastName = Console.ReadLine();

//            Console.Write("Email: ");
//            acc.Email = Console.ReadLine();

//            Console.Write("Company: ");
//            acc.Company = Console.ReadLine();

//            Console.Write("Date of Birth (yyyy-mm-dd): ");
//            acc.DateBirth = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");

//            Console.Write("Avatar path: ");
//            acc.Avatar = Console.ReadLine();

//            Console.Write("Status: ");
//            acc.AccountStatus = Console.ReadLine();

//            Console.Write("Password: ");
//            acc.AccountPassword = Console.ReadLine();

//            await _controller.UpdateAccountAsync(acc);
//        }

//        private async Task DeleteAccountAsync()
//        {
//            Console.Write("Enter Account Id to delete: ");
//            var id = int.Parse(Console.ReadLine() ?? "0");
//            await _controller.DeleteAccountAsync(id);
//        }
//    }
//}

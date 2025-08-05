using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.AccountService
{
    public interface IAccountService
    {
        Task<Account> AddAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(int id, Account account);
        Task<bool> DeleteAccountAsync(int id);
        Task<Account> GetAccountByIdAsync(int id);
    }
}

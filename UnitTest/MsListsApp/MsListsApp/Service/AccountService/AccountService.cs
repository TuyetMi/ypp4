using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Service.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAccountAsync(int id, Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            var existingAccount = await _context.Accounts.FindAsync(id);
            if (existingAccount == null)
            {
                return null;
            }

            existingAccount.FirstName = account.FirstName;
            existingAccount.LastName = account.LastName;
            existingAccount.Email = account.Email;
            existingAccount.Avatar = account.Avatar;
            existingAccount.DateBirth = account.DateBirth;
            existingAccount.Company = account.Company;
            existingAccount.AccountPassword = account.AccountPassword;
            existingAccount.AccountStatus = account.AccountStatus ?? "Active";

            _context.Accounts.Update(existingAccount);
            await _context.SaveChangesAsync();
            return existingAccount;
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return false;
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }
    }
}

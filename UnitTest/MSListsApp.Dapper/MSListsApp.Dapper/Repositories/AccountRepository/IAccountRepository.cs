
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        void EnsureTableAccountCreated();
        int Add(Account account);
        void Update(Account account);
        void Delete(int id);
        Account GetAccountInfoById(int accountId);
    }
}


using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        int Add(Account account);
        void CreateTable();
        Account GetById(int id);
    }
}


using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.AccountRepository
{
    public interface IAccountRepository
    {
        AccountSummaryDto? GetAccountInfoById(int id);
    }
}

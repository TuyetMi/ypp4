using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.AccountService
{
    public interface IAccountService
    {
        int CreateAccount(CreateAccountDto accountDto);
        void UpdateAccount(UpdateAccountDto accountDto);
        void DeleteAccount(int id);
        AccountDto GetAccountInfoById(int id);
    }
}

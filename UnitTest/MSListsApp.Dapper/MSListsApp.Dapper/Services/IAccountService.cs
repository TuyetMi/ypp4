using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services
{
    public interface IAccountService
    {
        void EnsureTableCreated();

        int CreateAccount(AccountCreateDto dto);

        void UpdateAccount(AccountUpdateDto dto);

        void DeleteAccount(int id);

        AccountReadDto GetAccountById(int id);
    }
}

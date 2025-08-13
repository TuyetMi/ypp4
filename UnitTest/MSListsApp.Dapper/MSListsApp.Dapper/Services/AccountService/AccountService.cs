using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.AccountRepository;

namespace MSListsApp.Dapper.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }
        public int CreateAccount(AccountDto dto)
        {
            var account = new Account
            {
                Avatar = dto.Avatar,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateBirth = dto.DateBirth,
                Email = dto.Email,
                Company = dto.Company,
                AccountStatus = dto.AccountStatus,
                AccountPassword = dto.AccountPassword
            };

            return _repository.Add(account);
        }

        public AccountDto? GetAccountInfoById(int id)
        {
            var account = _repository.GetById(id); // account kiểu Account (model)
            if (account == null) return null;

            // map sang DTO
            var dto = new AccountDto
            {
                Id = account.Id,
                Avatar = account.Avatar,
                FirstName = account.FirstName,
                LastName = account.LastName,
                DateBirth = account.DateBirth,
                Email = account.Email,
                Company = account.Company,
                AccountStatus = account.AccountStatus,
                AccountPassword = "" // không trả mật khẩu ra client
            };

            return dto;
        }

    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories;

namespace MSListsApp.Dapper.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _repository;

        public AccountService(AccountRepository repository)
        {
            _repository = repository;
        }

        public void EnsureTableCreated()
        {
            _repository.EnsureTableAccountCreated();
        }

        public int CreateAccount(AccountCreateDto dto)
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

        public void UpdateAccount(AccountUpdateDto dto)
        {
            var account = new Account
            {
                Id = dto.Id,
                Avatar = dto.Avatar,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateBirth = dto.DateBirth,
                Email = dto.Email,
                Company = dto.Company,
                AccountStatus = dto.AccountStatus,
                AccountPassword = dto.AccountPassword
            };

            _repository.Update(account);
        }

        public void DeleteAccount(int id)
        {
            _repository.Delete(id);
        }

        public AccountReadDto GetAccountById(int id)
        {
            var account = _repository.GetAccountInfoById(id);
            if (account == null) return null;

            return new AccountReadDto
            {
                Id = account.Id,
                Avatar = account.Avatar,
                FirstName = account.FirstName,
                LastName = account.LastName,
                DateBirth = account.DateBirth,
                Email = account.Email,
                Company = account.Company,
                AccountStatus = account.AccountStatus
            };
        }
    }

}

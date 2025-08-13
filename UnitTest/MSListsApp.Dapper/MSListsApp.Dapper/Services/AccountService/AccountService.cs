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
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        public int CreateAccount(CreateAccountDto dto)
        {
            ValidateCreate(dto);
            var account = MapToEntity(dto);
            return _accountRepository.Add(account);
        }

        public void UpdateAccount(UpdateAccountDto dto)
        {
            ValidateUpdate(dto);
            var account = MapToEntity(dto);
            _accountRepository.Update(account);
        }

        public void DeleteAccount(int id)
        {
            _accountRepository.Delete(id);
        }

        public AccountDto GetAccountInfoById(int id)
        {
            var account = _accountRepository.GetAccountInfoById(id);
            return account != null ? MapToDto(account) : null;
        }

        #region Mapping

        private AccountDto MapToDto(Account account)
        {
            return new AccountDto
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

        private Account MapToEntity(CreateAccountDto dto)
        {
            return new Account
            {
                Avatar = dto.Avatar,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateBirth = dto.DateBirth,
                Email = dto.Email,
                Company = dto.Company,
                AccountStatus = dto.AccountStatus
            };
        }

        private Account MapToEntity(UpdateAccountDto dto)
        {
            return new Account
            {
                Id = dto.Id,
                Avatar = dto.Avatar,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateBirth = dto.DateBirth,
                Email = dto.Email,
                Company = dto.Company,
                AccountStatus = dto.AccountStatus
            };
        }

        #endregion

        #region Validation

        private void ValidateCreate(CreateAccountDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");
        }

        private void ValidateUpdate(UpdateAccountDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("Invalid account ID.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email is required.");
        }

        #endregion
    }

}

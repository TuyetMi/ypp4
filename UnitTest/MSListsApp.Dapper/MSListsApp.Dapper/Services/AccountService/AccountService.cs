
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

        public AccountSummaryDto GetAccountInfoById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid account id.", nameof(id));

            var account = _repository.GetAccountInfoById(id);

            if (account == null)
                throw new KeyNotFoundException($"Account with Id {id} not found.");

            return account;
        }


    }

}

using MVC.Dtos.AccountDtos;
using MVC.Models;
using MVC.Repositories.AccountRepository;

namespace MVC.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        // CRUD
        public Task<int> CreateAsync(Account account) => _repository.CreateAsync(account);
        public Task<Account?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<IEnumerable<Account>> GetAllAsync() => _repository.GetAllAsync();
        public Task<int> UpdateAsync(Account account) => _repository.UpdateAsync(account);
        public Task<int> DeleteAsync(int id) => _repository.DeleteAsync(id);

        // DTO
        public Task<AccountInfoDto?> GetAccountInfoByIdAsync(int id) => _repository.GetAccountInfoByIdAsync(id);
        public Task<IEnumerable<AccountInfoDto>> GetAllAccountInfoAsync()
        {
            return _repository.GetAllAccountInfoAsync();
        }
    }
}

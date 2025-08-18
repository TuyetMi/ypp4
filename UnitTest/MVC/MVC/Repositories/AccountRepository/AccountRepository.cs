using System.Data;
using System.Data.SqlClient;
using Dapper;
using MVC.Dtos.AccountDtos;
using MVC.Models;

namespace MVC.Repositories.AccountRepository
{
    // Account repository implementation
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(IDbConnection connection)
                   : base(connection, "Account")
        {
        }

        public async Task<AccountInfoDto?> GetAccountInfoByIdAsync(int id)
        {
            const string sql = @"
                SELECT 
                    Id, Avatar, FirstName, LastName, DateBirth, Email, Company, Status
                FROM Account
                WHERE Id = @Id;
            ";

            return await _connection.QuerySingleOrDefaultAsync<AccountInfoDto>(sql, new { Id = id });
        }

        public async Task<IEnumerable<AccountInfoDto>> GetAllAccountInfoAsync()
        {
            const string sql = @"
                SELECT 
                    Id, Avatar, FirstName, LastName, DateBirth, Email, Company, Status
                FROM Account;
            ";

            return await _connection.QueryAsync<AccountInfoDto>(sql);
        }

    }
}

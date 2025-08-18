using System.Data;
using Dapper;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IDbConnection _connection;

        public AccountRepository(IDbConnection connection)
        {
            _connection = connection;

        }

        //public Account GetById(int id)
        //{
        //    var sql = @"
        //        SELECT 
        //            a.Id,
        //            a.FirstName,
        //            a.LastName,
        //            a.Avatar,
        //            a.Email,
        //            a.Company,
        //            a.AccountStatus
        //        FROM Account a
        //        WHERE a.Id = @Id;
        //    ";
        //    return _connection.QuerySingleOrDefault<Account>(sql, new { Id = id });
        //}

        public AccountSummaryDto GetAccountInfoById(int id)
        {
            var sql = @"
                SELECT 
                    Id,
                    FirstName,
                    LastName,
                    Avatar,
                    Email,
                    Company,
                    AccountStatus
                FROM Account
                WHERE Id = @Id;
            ";
            return _connection.QuerySingleOrDefault<AccountSummaryDto>(sql, new { Id = id });
        }
    }
}

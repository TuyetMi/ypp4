using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
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
        public void EnsureTableAccountCreated()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Account (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Avatar TEXT,
                    FirstName TEXT,
                    LastName TEXT,
                    DateBirth DATETIME,
                    Email TEXT,
                    Company TEXT,
                    AccountStatus TEXT
                );
            ";
            _connection.Execute(sql);
        }

        public int Add(Account account)
        {
            var sql = @"
                INSERT INTO Account (Avatar, FirstName, LastName, DateBirth, Email, Company, AccountStatus)
                VALUES (@Avatar, @FirstName, @LastName, @DateBirth, @Email, @Company, @AccountStatus);
                SELECT last_insert_rowid();
            ";
            return _connection.ExecuteScalar<int>(sql, account);
        }
        public void Update(Account account)
        {
            var sql = @"
                UPDATE Account SET
                    Avatar = @Avatar,
                    FirstName = @FirstName,
                    LastName = @LastName,
                    DateBirth = @DateBirth,
                    Email = @Email,
                    Company = @Company,
                    AccountStatus = @AccountStatus
                WHERE Id = @Id;
            ";
            _connection.Execute(sql, account);
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Account WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        public Account GetAccountInfoById(int accountId)
        {
            var sql = @"
                SELECT 
                    a.Id,
                    a.FirstName,
                    a.LastName,
                    a.Avatar,
                    a.Email,
                    a.Company,
                    a.AccountStatus
                FROM Account a
                WHERE a.Id = @AccountId;
            ";

            return _connection.QuerySingleOrDefault<Account>(sql, new { AccountId = accountId });
        }
    }
}

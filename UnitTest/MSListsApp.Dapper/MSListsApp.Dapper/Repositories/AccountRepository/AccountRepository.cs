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
                    AccountStatus TEXT,
                    AccountPassword TEXT
                );
            ";
            _connection.Execute(sql);
        }

        public int Add(Account account)
        {
            var sql = @"
                INSERT INTO Account 
                    (Avatar, FirstName, LastName, DateBirth, Email, Company, AccountStatus, AccountPassword)
                VALUES 
                    (@Avatar, @FirstName, @LastName, @DateBirth, @Email, @Company, @AccountStatus, @AccountPassword);
                SELECT last_insert_rowid();"; 

            var id = _connection.ExecuteScalar<int>(sql, account);
            return id;
        }

        public Account GetById(int id)
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
                WHERE a.Id = @Id;
            ";

            return _connection.QuerySingleOrDefault<Account>(sql, new { Id = id });
        }
    }
}

using System.Data;
using Dapper;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.FavoriteListRepository
{
    public class FavoriteListRepository : IFavoriteListRepository
    {
        private readonly IDbConnection _connection;

        public FavoriteListRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void CreateTable()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS FavoriteList (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ListId INTEGER NULL,
                    AccountId INTEGER NULL,
                    CreatedAt TEXT NULL,
                    UpdatedAt TEXT NULL
                );
            ";
            _connection.Execute(sql);
        }

        public int Add(FavoriteList favoriteList)
        {
            var sql = @"
                INSERT INTO FavoriteList (ListId, AccountId, CreatedAt, UpdatedAt)
                VALUES (@ListId, @AccountId, @CreatedAt, @UpdatedAt);
                SELECT last_insert_rowid();
            ";
            return _connection.ExecuteScalar<int>(sql, favoriteList);
        }

        public IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId)
        {
            var sql = @"
                SELECT 
                    l.ListName,
                    l.Icon,
                    l.Color
                FROM FavoriteList fl
                INNER JOIN List l ON fl.ListId = l.Id
                WHERE fl.AccountId = @AccountId
                    AND l.ListStatus = 'Active'
                ORDER BY fl.CreatedAt DESC;
            ";
            return _connection.Query<ListSummaryDto>(sql, new { AccountId = accountId });
        }
    }
}

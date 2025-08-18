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

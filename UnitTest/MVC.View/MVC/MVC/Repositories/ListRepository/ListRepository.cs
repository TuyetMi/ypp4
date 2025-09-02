using System.Data;
using System.Data.Common;
using Dapper;
using MVC.Dtos.ListDtos;
using MVC.Models;

namespace MVC.Repositories.ListRepository
{
    public class ListRepository: GenericRepository<List>, IListRepository
    {
        public ListRepository(IDbConnection connection) : base(connection, "List")
        {
        }

        public async Task<IEnumerable<ListInfoDto>> GetRecentListsByAccountAsync(int accountId)
        {
            var sql = @"
            SELECT  
                l.Id,
                l.ListName,
                l.Icon,
                l.Color,
                rl.LastAccessedAt,
                CASE WHEN fl.Id IS NOT NULL THEN 1 ELSE 0 END AS IsFavorited
            FROM List l
            JOIN RecentList rl ON l.Id = rl.ListId
            LEFT JOIN FavoriteList fl 
                ON fl.ListId = l.Id AND fl.FavoriteListOfAccount = @AccountId
            WHERE rl.AccountId = @AccountId
                AND l.ListStatus = @ActiveStatus
            ORDER BY rl.LastAccessedAt"
            ;

            var result = await _connection.QueryAsync<ListInfoDto>(
                 sql,
                 new
                 {
                     AccountId = accountId,
                     ActiveStatus = (int)ListStatus.Active  // dùng enum số explicit
                 }
             );
            return result;
        }
        public async Task<IEnumerable<ListInfoDto>> GetFavoritesListByAccountAsync(int accountId)
        {
            var sql = @"
            SELECT 
                l.Id,
                l.ListName,
                l.Icon,
                l.Color,
                CASE WHEN fl.Id IS NOT NULL THEN 1 ELSE 0 END AS IsFavorited
            FROM FavoriteList fl
            INNER JOIN List l ON fl.ListId = l.Id
            WHERE fl.FavoriteListOfAccount = @AccountId
                AND l.ListStatus = @ActiveStatus
            ORDER BY fl.CreatedAt DESC";

            // Nếu ListStatus enum dùng số explicit
            var result = await _connection.QueryAsync<ListInfoDto>(
                sql,
                new 
                {
                    AccountId = accountId, 
                    ActiveStatus = (int)ListStatus.Active 
                }
            );

            return result;
        }
    }
}


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

        public async Task<IEnumerable<ListInfoDto>> GetRecentListsByUserAsync(int accountId)
        {
            var sql = @"
            SELECT  
                l.Id,
                l.ListName,
                l.Icon,
                l.Color,
                rl.LastAccessedAt,
                CASE WHEN fvrl.Id IS NOT NULL THEN 1 ELSE 0 END AS IsFavorited
            FROM List l
            JOIN RecentList rl ON l.Id = rl.ListId
            LEFT JOIN FavoriteList fvrl 
                ON fvrl.ListId = l.Id AND fvrl.FavoriteListOfUser = @AccountId
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
        public async Task<IEnumerable<ListInfoDto>> GetFavoritesByUserAsync(int userId)
        {
            var sql = @"
            SELECT 
                l.Id,
                l.ListName,
                l.Icon,
                l.Color
            FROM FavoriteList fl
            INNER JOIN List l ON fl.ListId = l.Id
            WHERE fl.FavoriteListOfUser = @UserId
                AND l.ListStatus = @ActiveStatus
            ORDER BY fl.CreatedAt DESC";

            // Nếu ListStatus enum dùng số explicit
            var result = await _connection.QueryAsync<ListInfoDto>(
                sql,
                new 
                { 
                    UserId = userId, 
                    ActiveStatus = (int)ListStatus.Active 
                }
            );

            return result;
        }
    }
}


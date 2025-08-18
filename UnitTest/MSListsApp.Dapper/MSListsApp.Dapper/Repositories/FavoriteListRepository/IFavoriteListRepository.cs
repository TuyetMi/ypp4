using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.FavoriteListRepository
{
    public interface IFavoriteListRepository
    {

        IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId);
    }
}
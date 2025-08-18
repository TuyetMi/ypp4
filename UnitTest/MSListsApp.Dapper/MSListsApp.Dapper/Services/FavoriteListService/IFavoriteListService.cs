using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.FavoriteListService
{
    public interface IFavoriteListService
    {
        IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId);
    }
}
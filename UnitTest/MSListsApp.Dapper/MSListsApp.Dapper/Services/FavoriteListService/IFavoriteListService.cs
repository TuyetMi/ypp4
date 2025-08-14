using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.FavoriteListService
{
    public interface IFavoriteListService
    {
        int AddFavoriteList(FavoriteListDto favoriteListDto);
        IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId);
    }
}
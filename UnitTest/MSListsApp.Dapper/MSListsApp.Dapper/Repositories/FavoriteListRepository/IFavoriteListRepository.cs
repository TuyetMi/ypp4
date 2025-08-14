using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.FavoriteListRepository
{
    public interface IFavoriteListRepository
    {
        int Add(FavoriteList favoriteList);
        void CreateTable();
        IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId);
    }
}
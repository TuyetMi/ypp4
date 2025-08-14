
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Repositories.FavoriteListRepository;

namespace MSListsApp.Dapper.Services.FavoriteListService
{
    public class FavoriteListService : IFavoriteListService
    {
        private readonly FavoriteListRepository _repository;

        public FavoriteListService(FavoriteListRepository repository)
        {
            _repository = repository;
        }

        public int AddFavoriteList(FavoriteListDto favoriteListDto)
        {
            var favoriteList = new Models.FavoriteList
            {
                ListId = favoriteListDto.ListId,
                AccountId = favoriteListDto.AccountId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            return _repository.Add(favoriteList);
        }

        public IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId)
        {
            return _repository.GetFavoriteListsByUser(accountId);
        }
    }

}

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


        public IEnumerable<ListSummaryDto> GetFavoriteListsByUser(int accountId)
        {
            return _repository.GetFavoriteListsByUser(accountId);
        }
    }

}
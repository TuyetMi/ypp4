using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListRepository;

namespace MSListsApp.Dapper.Services.ListService
{
    public class ListService : IListService
    {
        private readonly ListRepository _repository;

        public ListService(ListRepository repository)
        {
            _repository = repository;
        }

        public ListDetailDto? GetDetailById(int id)
        {
            return _repository.GetDetailById(id);
        }

        public IEnumerable<ListSummaryDto> GetMyList(int accountId)
        {
            return _repository.GetMyList(accountId);
        }

    }
}

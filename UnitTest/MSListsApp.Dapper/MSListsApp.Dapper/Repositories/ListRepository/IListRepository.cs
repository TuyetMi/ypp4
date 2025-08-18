using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.ListRepository
{
    public interface IListRepository
    {
        ListDetailDto? GetDetailById(int id);
        IEnumerable<ListSummaryDto> GetMyList(int accountId);
    }
}
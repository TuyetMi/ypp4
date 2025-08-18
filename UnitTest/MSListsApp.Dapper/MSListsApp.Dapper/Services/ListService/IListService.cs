using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.ListService
{
    public interface IListService
    {

        ListDetailDto? GetDetailById(int id);
        IEnumerable<ListSummaryDto> GetMyList(int accountId);
    }
}
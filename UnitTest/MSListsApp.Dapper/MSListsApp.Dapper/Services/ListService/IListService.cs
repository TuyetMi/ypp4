using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.ListService
{
    public interface IListService
    {
        int CreateList(ListDto dto);
        ListDetailDto? GetDetailById(int id);
        IEnumerable<ListSummaryDto> GetListsInPersonalWorkspaceByUser(int accountId);
    }
}
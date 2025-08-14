using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.ListRepository
{
    public interface IListRepository
    {
        int Add(List list);
        void CreateTable();
        
        ListDetailDto? GetDetailById(int id);
        IEnumerable<ListSummaryDto> GetListsInPersonalWorkspaceByUser(int accountId);
    }
}
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.ListService
{
    public interface IListService
    {
        int CreateList(ListDto dto);
        ListDto? GetListById(int id);
        object? GetListInfoById(int id);
    }
}
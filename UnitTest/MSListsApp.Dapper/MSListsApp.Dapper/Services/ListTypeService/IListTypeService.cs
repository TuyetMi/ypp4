using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.ListTypeService
{
    public interface IListTypeService
    {
        int CreateListType(ListTypeDto dto);
        IEnumerable<ListTypeDto> GetAllListTypes();
        ListTypeDto? GetListTypeById(int id);
    }
}
using MSListsApp.Dapper.DTOs;

namespace MSListsApp.Dapper.Services.ListTypeService
{
    public interface IListTypeService
    {

        IEnumerable<ListTypeDto> GetAllListTypes();
        ListTypeDto? GetListTypeById(int id);
    }
}
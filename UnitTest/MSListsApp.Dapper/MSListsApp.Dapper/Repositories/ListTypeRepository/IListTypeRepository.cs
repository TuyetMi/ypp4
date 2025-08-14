using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.ListTypeRepository
{
    public interface IListTypeRepository
    {
        int Add(ListType listType);
        void CreateTable();
        IEnumerable<ListTypeDto> GetAll();
        ListTypeDto? GetById(int id);
    }
}
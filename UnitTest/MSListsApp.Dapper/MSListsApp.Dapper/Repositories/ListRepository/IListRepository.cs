using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.ListRepository
{
    public interface IListRepository
    {
        int Add(List list);
        void EnsureTableListCreated();
        object? GetListInfoById(int id);
    }
}
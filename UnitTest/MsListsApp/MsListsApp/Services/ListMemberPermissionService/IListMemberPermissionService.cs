using MsListsApp.Models;

namespace MsListsApp.Services.ListMemberPermissionService
{
    public interface IListMemberPermissionService
    {
        Task<ListMemberPermission> GetListMemberPermissionByIdAsync(int id);
        Task<IEnumerable<ListMemberPermission>> GetAllListMemberPermissionsAsync();
        Task<ListMemberPermission> CreateListMemberPermissionAsync(ListMemberPermission permission);
        Task<ListMemberPermission> UpdateListMemberPermissionAsync(ListMemberPermission permission);
        Task<bool> DeleteListMemberPermissionAsync(int id);
    }
}

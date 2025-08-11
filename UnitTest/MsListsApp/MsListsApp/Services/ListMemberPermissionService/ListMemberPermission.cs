using MsListsApp.Models;

namespace MsListsApp.Services.ListMemberPermissionService
{
    public class ListMemberPermission: IListMemberPermissionService
    {
        private readonly List<Account> _accounts;
        private readonly List<List> _lists;
        private readonly List<ListPermission> _listPermissions;
        private readonly List<ListMemberPermission> _listMemberPermissions;


        public ListMemberPermissionService(
            List<Account> accounts,
            List<List> lists,
            List<ListPermission> listPermissions,
            List<ListMemberPermission> listMemberPermissions)
        {
            _accounts = accounts ?? new List<Account>();
            _lists = lists ?? new List<List>();
            _listPermissions = listPermissions ?? new List<ListPermission>();
            _listMemberPermissions = listMemberPermissions ?? new List<ListMemberPermission>();
        }


        public async Task<IEnumerable<ListMemberPermission>> GetPermissionsByUserAndListAsync(int accountId, int listId)
        {
            if (_accounts == null)
                throw new InvalidOperationException("Accounts collection is null");
            if (_lists == null)
                throw new InvalidOperationException("Lists collection is null");
            if (_listMemberPermissions == null)
                throw new InvalidOperationException("ListMemberPermissions collection is null");

            if (_accounts.FirstOrDefault(a => a.Id == accountId) == null)
                throw new Exception("Account not found");
            if (_lists.FirstOrDefault(l => l.Id == listId) == null)
                throw new Exception("List not found");

            var permissions = _listMemberPermissions
                .Where(p => p.AccountId == accountId && p.ListId == listId)
                .OrderByDescending(p => p.CreatedAt ?? DateTime.MinValue) // Xử lý CreatedAt null
                .ToList();

            return await Task.FromResult(permissions);
        }

        public async Task AddOrUpdatePermissionAsync(int listId, int accountId, int highestPermissionId, int grantedByAccountId, string? note = null)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!_accounts.Any(a => a.Id == accountId))
                throw new Exception("Account not found");
            if (!_accounts.Any(a => a.Id == grantedByAccountId))
                throw new Exception("GrantedBy account not found");
            if (!_lists.Any(l => l.Id == listId))
                throw new Exception("List not found");
            if (!_listPermissions.Any(p => p.Id == highestPermissionId))
                throw new Exception("Permission not found");

            // Lấy PermissionCode từ highestPermissionId
            var permission = _listPermissions.First(p => p.Id == highestPermissionId);

            // Kiểm tra xem đã có quyền cho user trên danh sách này chưa
            var existingPermission = _listMemberPermissions
                .FirstOrDefault(p => p.ListId == listId && p.AccountId == accountId);

            if (existingPermission != null)
            {
                // Cập nhật quyền hiện có
                existingPermission.HighestPermissionId = highestPermissionId;
                existingPermission.HighestPermissionCode = permission.PermissionCode;
                existingPermission.GrantedByAccountId = grantedByAccountId;
                existingPermission.Note = note;
                existingPermission.UpdatedAt = DateTime.Now;
            }
            else
            {
                // Thêm quyền mới
                var newPermission = new ListMemberPermission
                {
                    Id = _listMemberPermissions.Any() ? _listMemberPermissions.Max(p => p.Id) + 1 : 1, // Giả lập IDENTITY
                    ListId = listId,
                    AccountId = accountId,
                    HighestPermissionId = highestPermissionId,
                    HighestPermissionCode = permission.PermissionCode,
                    GrantedByAccountId = grantedByAccountId,
                    Note = note,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _listMemberPermissions.Add(newPermission);
            }

            await Task.CompletedTask;
        }

        public async Task<string?> GetHighestPermissionCodeAsync(int accountId, int listId)
        {
            // Kiểm tra tài khoản và danh sách
            if (!_accounts.Any(a => a.Id == accountId))
                throw new Exception("Account not found");
            if (!_lists.Any(l => l.Id == listId))
                throw new Exception("List not found");

            // Lấy quyền cao nhất của user trên danh sách
            var permission = _listMemberPermissions
                .Where(p => p.AccountId == accountId && p.ListId == listId)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefault();

            return await Task.FromResult(permission?.HighestPermissionCode);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.ListService
{
    public class ListService : IListService
    {
        private readonly List<Account> _accounts;
        private readonly List<RecentList> _recentLists;

        public ListService(List<Account> accounts, List<RecentList> recentLists)
        {
            _accounts = accounts ?? throw new ArgumentNullException(nameof(accounts));
            _recentLists = recentLists ?? throw new ArgumentNullException(nameof(recentLists));
        }

        public async Task<IEnumerable<RecentList>> GetRecentListsByUserAsync(int accountId)
        {
            // Kiểm tra xem tài khoản có tồn tại không
            var account = _accounts.FirstOrDefault(a => a.Id == accountId);
            if (account == null)
            {
                throw new Exception("Account not found");
            }

            // Lấy danh sách RecentList theo accountId, sắp xếp theo LastAccessedAt giảm dần
            var result = _recentLists
                .Where(rl => rl.AccountId == accountId)
                .OrderByDescending(rl => rl.LastAccessedAt)
                .ToList();

            return await Task.FromResult(result);
        }
    }
}

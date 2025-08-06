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
        private readonly AppDbContext _context;

        public ListService(AppDbContext context)
        {
            _context = context;
        }

        public List<List> GetRecentListsByUser(int userId)
        {
            var listIdsWithPermission = _context.ListMemberPermissions
                .Where(p => p.AccountId == userId)
                .Select(p => p.ListId)
                .Distinct();

            var recentLists = _context.RecentLists
                .Where(r => r.AccountId == userId)
                .Select(r => r.ListId)
                .Where(l => l.ListStatus == "Active" && listIdsWithPermission.Contains(l.Id))
                .OrderByDescending(l => l.UpdatedAt)
                .ToList();

            return recentLists;
        }
    }
}

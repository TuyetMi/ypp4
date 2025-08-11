using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MsListsApp.Models;

namespace MsListsApp.Services.ListService
{
    public interface IListService
    {
        Task<IEnumerable<RecentList>> GetRecentListsByUserAsync(int accountId);
    }
}

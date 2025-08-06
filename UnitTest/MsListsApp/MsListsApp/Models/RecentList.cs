using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsListsApp.Models
{
    public class RecentList
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ListId { get; set; }
        public DateTime LastAccessedAt { get; set; }

    }
}

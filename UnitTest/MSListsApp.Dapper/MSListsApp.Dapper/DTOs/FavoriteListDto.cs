using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.DTOs
{
    public class FavoriteListDto
    {
        public int Id { get; set; }
        public int? ListId { get; set; }
        public int? AccountId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

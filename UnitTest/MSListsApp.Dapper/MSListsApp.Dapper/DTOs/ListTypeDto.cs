using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.DTOs
{
    public class ListTypeDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Icon { get; set; }
        public string? ListTypeDescription { get; set; }
        public string? HeaderImage { get; set; }
    }
}

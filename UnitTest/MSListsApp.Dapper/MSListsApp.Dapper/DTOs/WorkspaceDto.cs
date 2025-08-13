using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSListsApp.Dapper.DTOs
{
    // Dùng để trả dữ liệu ra client hoặc hiển thị
    public class WorkspaceDto
    {
        public int Id { get; set; } // 0 khi tạo mới
        public string WorkspaceName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsPersonal { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }



}

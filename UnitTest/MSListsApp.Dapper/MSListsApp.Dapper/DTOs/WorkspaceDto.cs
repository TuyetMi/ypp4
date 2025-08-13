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
        public int Id { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsPersonal { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    // Dùng khi tạo mới workspace
    public class WorkspaceCreateDto
    {
        public string WorkspaceName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsPersonal { get; set; } = false;
    }

    // Dùng khi update workspace
    public class WorkspaceUpdateDto
    {
        public int Id { get; set; } // cần Id để xác định workspace
        public string? WorkspaceName { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

}

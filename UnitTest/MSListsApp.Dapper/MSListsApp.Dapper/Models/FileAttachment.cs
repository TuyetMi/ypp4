
namespace MSListsApp.Dapper.Models
{
    public class FileAttachment
    {
        public int Id { get; set; }
        public int? ListRowId { get; set; }
        public string? FileAttachmentName { get; set; }
        public string? FileUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FileAttachmentStatus { get; set; } = "Active";
    }
}

namespace MVC.Dtos.WorkspaceDtos
{
    public class WorkspaceInfoDto
    {
        public int Id { get; set; }
        public string WorkspaceName { get; set; } = null!;
        public bool IsPersonal { get; set; }
    }
}
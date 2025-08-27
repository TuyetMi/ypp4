using System.Text.Json;
using MVC.Services.WorkspaceService;

namespace MVC.Controllers
{
    public class WorkspaceController
    {
        private readonly IWorkspaceService _workspaceService;

        public WorkspaceController(IWorkspaceService workspaceService)
        {
            _workspaceService = workspaceService;
        }

        // Xử lý request lấy workspace cá nhân
        public async Task<string> GetPersonalWorkspace(int accountId)
        {
            var workspace = await _workspaceService.GetPersonalWorkspaceAsync(accountId);

            if (workspace == null)
            {
                // Trả về JSON thông báo không tìm thấy
                var notFound = new { Message = "Personal workspace not found" };
                return JsonSerializer.Serialize(notFound);
            }

            // Trả về JSON workspace
            return JsonSerializer.Serialize(workspace);
        }
    }
}

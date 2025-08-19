
using MVC.Models;
using MVC.Repositories.WorkspaceRepository;

namespace MVC.Services.WorkspaceService
{
    public class WorkspaceService : IWorkspaceService
    {
        private readonly IWorkspaceRepository _repository;
        public WorkspaceService(IWorkspaceRepository repository)
        {
            _repository = repository;
        }

        // CRUD
        public Task<int> CreateAsync(Workspace workspace) => _repository.CreateAsync(workspace);
        public Task<Workspace?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);
        public Task<IEnumerable<Workspace>> GetAllAsync() => _repository.GetAllAsync();
        public Task<int> UpdateAsync(Workspace workspace) => _repository.UpdateAsync(workspace);
        public Task<int> DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}


using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListRepository;

namespace MSListsApp.Dapper.Services.ListService
{
    public class ListService : IListService
    {
        private readonly ListRepository _repository;

        public ListService(ListRepository repository)
        {
            _repository = repository;
        }

        // Tạo List mới
        public int CreateList(ListDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ListName))
                throw new ArgumentException("ListName is required.");

            var list = new List
            {
                ListTypeId = dto.ListTypeId,
                ListTemplateId = dto.ListTemplateId,
                WorkspaceId = dto.WorkspaceId,
                ListName = dto.ListName,
                Icon = dto.Icon,
                Color = dto.Color,
                CreatedBy = dto.CreatedBy,
                CreatedAt = dto.CreatedAt ?? DateTime.UtcNow,
                ListStatus = dto.ListStatus
            };

            return _repository.Add(list);
        }

        public ListDetailDto? GetDetailById(int id)
        {
            return _repository.GetDetailById(id);
        }

        public IEnumerable<ListSummaryDto> GetListsInPersonalWorkspaceByUser(int accountId)
        {
            return _repository.GetListsInPersonalWorkspaceByUser(accountId);
        }

    }
}

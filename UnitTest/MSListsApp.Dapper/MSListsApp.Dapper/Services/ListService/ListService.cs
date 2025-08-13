using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Lấy List theo Id
        public ListDto? GetListById(int id)
        {
            var list = _repository.GetById(id);
            if (list == null) return null;

            return new ListDto
            {
                Id = list.Id,
                ListTypeId = list.ListTypeId,
                ListTemplateId = list.ListTemplateId,
                WorkspaceId = list.WorkspaceId,
                ListName = list.ListName,
                Icon = list.Icon,
                Color = list.Color,
                CreatedBy = list.CreatedBy,
                CreatedAt = list.CreatedAt,
                ListStatus = list.ListStatus
            };
        }

        // Lấy thông tin List (tên, icon, color, workspace name)
        public object? GetListInfoById(int id)
        {
            return _repository.GetListInfoById(id);
        }
    }
}

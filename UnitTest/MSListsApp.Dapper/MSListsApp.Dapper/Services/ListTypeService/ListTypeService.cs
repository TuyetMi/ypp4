
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListTypeRepository;

namespace MSListsApp.Dapper.Services.ListTypeService
{
    public class ListTypeService : IListTypeService
    {
        private readonly IListTypeRepository _repository;

        public ListTypeService(IListTypeRepository repository)
        {
            _repository = repository;
        }
        public int CreateListType(ListTypeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required.");

            var listType = new ListType
            {
                Title = dto.Title,
                Icon = dto.Icon,
                ListTypeDescription = dto.ListTypeDescription,
                HeaderImage = dto.HeaderImage
            };

            return _repository.Add(listType);
        }

        public ListTypeDto? GetListTypeById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<ListTypeDto> GetAllListTypes()
        {
            return _repository.GetAll();
        }

    }
}

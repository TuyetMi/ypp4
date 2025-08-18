using System.Data;
using Dapper;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;


namespace MSListsApp.Dapper.Repositories.ListTypeRepository
{
    public class ListTypeRepository: IListTypeRepository
    {
        private readonly IDbConnection _connection;

        public ListTypeRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public ListTypeDto? GetById(int id)
        {
            var sql = @"
                SELECT
                    Id,
                    Title, 
                    Icon, 
                    ListTypeDescription, 
                    HeaderImage
                FROM ListType
                WHERE Id = @Id;";
            return _connection.QueryFirstOrDefault<ListTypeDto>(sql, new { Id = id });
        }
        public IEnumerable<ListTypeDto> GetAll()
        {
            var sql = @"
                SELECT 
                    Title, 
                    Icon, 
                    ListTypeDescription, 
                    HeaderImage
                FROM ListType;";
            return _connection.Query<ListTypeDto>(sql);
        }

    }
}

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

        public void CreateTable()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS ListType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NULL,
                    Icon TEXT NULL,
                    ListTypeDescription TEXT NULL,
                    HeaderImage TEXT NULL
                );";
            _connection.Execute(sql);
        }

        public int Add(ListType listType)
        {
            var sql = @"
                INSERT INTO ListType 
                    (Title, Icon, ListTypeDescription, HeaderImage)
                VALUES 
                    (@Title, @Icon, @ListTypeDescription, @HeaderImage);
                SELECT last_insert_rowid();";
            return _connection.ExecuteScalar<int>(sql, listType);
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

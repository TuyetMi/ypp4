
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;


namespace MVC.Repositories
{
    // Generic repository implementation
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IDbConnection _connection;
        protected readonly string _tableName;
        protected readonly List<PropertyInfo> _properties;

        // Constructor nhận connection bên ngoài
        public GenericRepository(IDbConnection connection, string tableName)
        {
            _connection = connection;
            _tableName = tableName;

            _properties = typeof(T).GetProperties()
                                   .Where(p => p.Name != "Id")
                                   .ToList();
        }

        public async Task<int> CreateAsync(T entity)
        {
            var columnNames = string.Join(", ", _properties.Select(p => p.Name));
            var paramNames = string.Join(", ", _properties.Select(p => "@" + p.Name));

            // SQLite in-memory dùng last_insert_rowid()
            var sql = $@"
                INSERT INTO {_tableName} ({columnNames})
                VALUES ({paramNames});
                SELECT last_insert_rowid();
            ";

            return await _connection.ExecuteScalarAsync<int>(sql, entity);
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var columnNames = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));

            var sql = $@"
                SELECT {columnNames}
                FROM {_tableName}
                WHERE Id = @Id;
            ";

            return await _connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var columnNames = string.Join(", ", typeof(T).GetProperties().Select(p => p.Name));

            var sql = $@"
                SELECT {columnNames}
                FROM {_tableName};
            ";

            return await _connection.QueryAsync<T>(sql);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var setClause = string.Join(", ", _properties.Select(p => $"{p.Name} = @{p.Name}"));

            var sql = $@"
                UPDATE {_tableName}
                SET {setClause}
                WHERE Id = @Id;
            ";

            return await _connection.ExecuteAsync(sql, entity);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = $@"
                DELETE FROM {_tableName}
                WHERE Id = @Id;
            ";

            return await _connection.ExecuteAsync(sql, new { Id = id });
        }
    }
}

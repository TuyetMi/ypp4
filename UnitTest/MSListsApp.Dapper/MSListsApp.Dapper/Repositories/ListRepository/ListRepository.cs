using MSListsApp.Dapper.Models;
using System.Data;
using Dapper;



namespace MSListsApp.Dapper.Repositories.ListRepository
{
    public class ListRepository : IListRepository
    {
        private readonly IDbConnection _connection;

        public ListRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        // Tạo bảng nếu chưa có
        public void EnsureTableListCreated()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS List (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ListTypeId INTEGER NOT NULL,
                    ListTemplateId INTEGER,
                    WorkspaceId INTEGER,
                    ListName TEXT NOT NULL,
                    Icon TEXT,
                    Color TEXT,
                    CreatedBy INTEGER NOT NULL,
                    CreatedAt TEXT,
                    ListStatus TEXT NOT NULL DEFAULT 'Active'
                );";
            _connection.Execute(sql);
        }

        // Thêm List mới, trả về Id
        public int Add(List list)
        {
            var sql = @"
                INSERT INTO List 
                    (ListTypeId, ListTemplateId, WorkspaceId, ListName, Icon, Color, CreatedBy, CreatedAt, ListStatus)
                VALUES 
                    (@ListTypeId, @ListTemplateId, @WorkspaceId, @ListName, @Icon, @Color, @CreatedBy, @CreatedAt, @ListStatus);
                SELECT last_insert_rowid();";

            return _connection.ExecuteScalar<int>(sql, list);
        }

        // Lấy List theo Id
        public List? GetById(int id)
        {
            var sql = @"
                SELECT 
                    Id, 
                    ListTypeId, 
                    ListTemplateId, 
                    WorkspaceId, 
                    ListName, 
                    Icon, 
                    Color, 
                    CreatedBy, 
                    CreatedAt, 
                    ListStatus
                FROM List
                WHERE Id = @Id;";
            return _connection.QuerySingleOrDefault<List>(sql, new { Id = id });
        }

        // Lấy thông tin List (tên, icon, color, workspace name)
        public object? GetListInfoById(int id)
        {
            var sql = @"
                SELECT 
                    l.ListName,
                    l.Icon,
                    l.Color,
                    w.WorkspaceName
                FROM List l
                LEFT JOIN Workspace w ON l.WorkspaceId = w.Id
                WHERE l.Id = @Id;";

            return _connection.QuerySingleOrDefault(sql, new { Id = id });
        }
    }
}
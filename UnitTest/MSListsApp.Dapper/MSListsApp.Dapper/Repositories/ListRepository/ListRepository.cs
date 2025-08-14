using MSListsApp.Dapper.Models;
using System.Data;
using Dapper;
using MSListsApp.Dapper.DTOs;



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
        public void CreateTable()
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

       
        public ListDetailDto? GetDetailById(int id)
        {
            var sql = @"
                SELECT 
                    l.Id,
                    l.ListName,
                    l.Icon,
                    l.Color,
                    w.WorkspaceName,
                    l.ListStatus
                FROM List l
                LEFT JOIN Workspace w ON l.WorkspaceId = w.Id
                WHERE l.Id = @Id;
            ";
            return _connection.QueryFirstOrDefault<ListDetailDto>(sql, new { Id = id });
        }
        public IEnumerable<ListSummaryDto> GetListsInPersonalWorkspaceByUser(int accountId)
        {
            var sql = @"
                SELECT 
                    l.ListName,
                    l.Icon,
                    l.Color,
                    ws.WorkspaceName,
	                CASE WHEN fvrl.Id IS NOT NULL THEN 1 ELSE 0 END AS IsFavorited
                FROM List l
                JOIN Workspace ws ON l.WorkspaceID = ws.Id
                LEFT JOIN FavoriteList fvrl
                    ON fvrl.ListId = l.Id 
                    AND fvrl.AccountId = @AccountId  
                WHERE ws.CreatedBy = @AccountId
                    AND l.ListStatus = 'Active'
                    AND ws.IsPersonal = 1;
            ";

            return _connection.Query<ListSummaryDto>(sql, new { AccountId = accountId });
        }
    }
}
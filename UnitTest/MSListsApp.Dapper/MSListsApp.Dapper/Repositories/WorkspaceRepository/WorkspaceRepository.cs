
using System.Data;
using Dapper;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.WorkspaceRepository
{
    public class WorkspaceRepository: IWorkspaceRepository
    {
        private readonly IDbConnection _connection;

        public WorkspaceRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public void EnsureTableWorkspaceCreated()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS Workspace (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    WorkspaceName TEXT,
                    CreatedBy INTEGER,
                    IsPersonal BOOLEAN,
                    CreatedAt DATETIME,
                    UpdatedAt DATETIME
                );
            ";
            _connection.Execute(sql);
        }

        public int Add(Workspace workspace)
        {
            var sql = @"
                INSERT INTO Workspace 
                    (WorkspaceName, CreatedBy, IsPersonal, CreatedAt, UpdatedAt)
                VALUES 
                    (@WorkspaceName, @CreatedBy, @IsPersonal, @CreatedAt, @UpdatedAt);
                SELECT last_insert_rowid();";

            return _connection.ExecuteScalar<int>(sql, workspace);
        }

        public Workspace GetWorkspaceById(int id)
        {
            var sql = @"
                SELECT 
                    Id,
                    WorkspaceName,
                    CreatedBy,
                    IsPersonal,
                    CreatedAt,
                    UpdatedAt
                FROM Workspace
                WHERE Id = @Id;
            ";
            return _connection.QuerySingleOrDefault<Workspace>(sql, new { Id = id });
        }
        public IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId)
        {
            var sql = @"
                SELECT WorkspaceName 
                FROM Workspace 
                WHERE CreatedBy = @AccountId;";
            return _connection.Query<string>(sql, new { AccountId = accountId });
        }
    }
}

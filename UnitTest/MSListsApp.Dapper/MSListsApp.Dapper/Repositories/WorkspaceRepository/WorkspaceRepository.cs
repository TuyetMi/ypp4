using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                INSERT INTO Workspace (WorkspaceName, CreatedBy, IsPersonal, CreatedAt, UpdatedAt)
                VALUES (@WorkspaceName, @CreatedBy, @IsPersonal, @CreatedAt, @UpdatedAt);
                SELECT last_insert_rowid();
            ";
            return _connection.ExecuteScalar<int>(sql, workspace);
        }

        public void Update(Workspace workspace)
        {
            var sql = @"
                UPDATE Workspace SET
                    WorkspaceName = @WorkspaceName,
                    CreatedBy = @CreatedBy,
                    IsPersonal = @IsPersonal,
                    CreatedAt = @CreatedAt,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id;
            ";
            _connection.Execute(sql, workspace);
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM Workspace WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }

        public Workspace GetWorkspaceById(int workspaceId)
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
                WHERE Id = @WorkspaceId;
            ";
            return _connection.QuerySingleOrDefault<Workspace>(sql, new { WorkspaceId = workspaceId });
        }
        public IEnumerable<Workspace> GetWorkspacesByCreatorId(int accountId)
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
                WHERE CreatedBy = @AccountId;
            ";
            return _connection.Query<Workspace>(sql, new { AccountId = accountId });
        }
    }
}


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

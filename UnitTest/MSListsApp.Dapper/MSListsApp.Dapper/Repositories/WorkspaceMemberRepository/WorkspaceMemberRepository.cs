
using System.Data;
using Dapper;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper.Repositories.WorkspaceMemberRepository
{
    public class WorkspaceMemberRepository : IWorkspaceMemberRepository
    {
        private readonly IDbConnection _connection;

        public WorkspaceMemberRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public WorkspaceMember? GetById(int id)
        {
            var sql = @" SELECT 
                    Id,
                    WorkspaceId,
                    AccountId,
                    JoinedAt,
                    MemberStatus,
                    UpdatedAt
                FROM Workspace
                WHERE Id = @Id;
            ";
            return _connection.QuerySingleOrDefault<WorkspaceMember>(sql, new { Id = id });
        }
        public IEnumerable<string> GetAccountNamesByWorkspaceId(int workspaceId)
        {
            var sql = @"
                SELECT a.FirstName || ' ' || a.LastName AS FullName
                FROM WorkspaceMember wm
                INNER JOIN Account a ON wm.AccountId = a.Id
                WHERE wm.WorkspaceId = @WorkspaceId;";
            return _connection.Query<string>(sql, new { WorkspaceId = workspaceId });
        }
        public IEnumerable<string> GetWorkspaceNamesByAccountId(int accountId)
        {
            var sql = @"
                SELECT w.WorkspaceName
                FROM WorkspaceMember wm
                INNER JOIN Workspace w ON wm.WorkspaceId = w.Id
                WHERE wm.AccountId = @AccountId;";
            return _connection.Query<string>(sql, new { AccountId = accountId });
        }
    }
}

using System.Data;
using Dapper;
using MVC.Dtos.WorkspaceDtos;
using MVC.Models;


namespace MVC.Repositories.WorkspaceRepository
{
    public class WorkspaceRepository : GenericRepository<Workspace>, IWorkspaceRepository
    {
        public WorkspaceRepository(IDbConnection connection)
           : base(connection, "Workspace")
        {
        }

        public async Task<WorkspaceInfoDto?> GetWorkSpaceInfoByIdAsync(int id)
        {
            var sql = @"
                SELECT 
                      Id,
                      WorkspaceName,
                      IsPersonal
                FROM Workspace
                WHERE Id = @Id
            ";

            return await _connection.QueryFirstOrDefaultAsync<WorkspaceInfoDto>(sql, new { Id = id });
        }

        public async Task<WorkspaceInfoDto?> GetPersonalWorkspaceAsync(int accountid)
        {
            var sql = @"
                SELECT 
                      Id,
                      WorkspaceName,
                      IsPersonal
                FROM Workspace
                WHERE CreatedBy = @AccountId
                  AND IsPersonal = 1
            ";
            return await _connection.QueryFirstOrDefaultAsync<WorkspaceInfoDto>(sql, new { AccountId = accountid });
        }
    }
}
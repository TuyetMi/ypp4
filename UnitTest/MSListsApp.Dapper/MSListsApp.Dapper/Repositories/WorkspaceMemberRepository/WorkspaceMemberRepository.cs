using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MSListsApp.Dapper.DTOs;
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

        public void EnsureTableWorkspaceMemberCreated()
        {
            var sql = @"
                CREATE TABLE IF NOT EXISTS WorkspaceMember (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    WorkspaceId INTEGER NOT NULL,
                    AccountId INTEGER NOT NULL,
                    JoinedAt DATETIME,
                    MemberStatus TEXT NOT NULL DEFAULT 'Active',
                    UpdatedAt DATETIME
                );
            ";
            _connection.Execute(sql);
        }

        public int Add(WorkspaceMember workspaceMember)
        {
            var sql = @"
                INSERT INTO WorkspaceMember (WorkspaceId, AccountId, JoinedAt, MemberStatus, UpdatedAt)
                VALUES (@WorkspaceId, @AccountId, @JoinedAt, @MemberStatus, @UpdatedAt);
                SELECT last_insert_rowid();
            ";
            return _connection.ExecuteScalar<int>(sql, workspaceMember);
        }

        public void Update(WorkspaceMemberDto workspaceMember)
        {
            var sql = @"
                UPDATE WorkspaceMember SET
                    WorkspaceId = @WorkspaceId,
                    AccountId = @AccountId,
                    JoinedAt = @JoinedAt,
                    MemberStatus = @MemberStatus,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id;
            ";
            _connection.Execute(sql, workspaceMember);
        }

        public void Delete(int id)
        {
            var sql = "DELETE FROM WorkspaceMember WHERE Id = @Id";
            _connection.Execute(sql, new { Id = id });
        }
        public WorkspaceMemberDto? GetById(int id)
        {
            var sql = @"
                SELECT 
                    Id,
                    WorkspaceId,
                    AccountId,
                    JoinedAt,
                    MemberStatus,
                    UpdatedAt
                FROM WorkspaceMember
                WHERE Id = @Id
            ";
            return _connection.QueryFirstOrDefault<WorkspaceMemberDto>(sql, new { Id = id });
        }





    }
}

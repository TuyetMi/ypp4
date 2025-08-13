using System;
using System.Collections.Generic;
using System.Data;


using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;
using SQLitePCL;
using MSListsApp.Dapper.Models;

namespace MSListsApp.Dapper
{
    public static class TestDatabaseHelper
    {
        public static SqliteConnection CreateInMemoryDatabase()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            return connection;
        }

        public static void CreateAllTables(IDbConnection connection)
        {
            // Ở đây gọi EnsureTable của tất cả repo
            var accountRepo = new AccountRepository(connection);
            accountRepo.EnsureTableAccountCreated();

            var workspaceRepo = new WorkspaceRepository(connection);
            workspaceRepo.EnsureTableWorkspaceCreated();

            var workspaceMemberRepo = new WorkspaceMemberRepository(connection);
            workspaceMemberRepo.EnsureTableWorkspaceMemberCreated();

        }

        public static void SeedData(IDbConnection connection)
        {
            // Seed Accounts
            var accountRepo = new AccountRepository(connection);
            accountRepo.Add(new Account
            {
                Avatar = "avatar1.png",
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Company = "CompanyA",
                AccountStatus = "Active",
                AccountPassword = "123456"
            });
            accountRepo.Add(new Account
            {
                Avatar = "avatar2.png",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                Company = "CompanyB",
                AccountStatus = "Active",
                AccountPassword = "abcdef"
            });

            // Seed Workspaces
            var workspaceRepo = new WorkspaceRepository(connection);
            var ws1Id = workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "Workspace 1",
                CreatedBy = 1,
                IsPersonal = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            var ws2Id = workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "Workspace 2",
                CreatedBy = 2,
                IsPersonal = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            // Seed WorkspaceMembers
            var wmRepo = new WorkspaceMemberRepository(connection);
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = ws1Id,
                AccountId = 1,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = ws1Id,
                AccountId = 2,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = ws2Id,
                AccountId = 2,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
        }
    }
}

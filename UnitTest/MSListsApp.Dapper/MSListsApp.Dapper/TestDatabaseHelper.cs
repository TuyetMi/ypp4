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
            var accountRepo = new AccountRepository(connection);

            // Thêm một vài account mẫu
            accountRepo.Add(new Account
            {
                Avatar = "avatar1.png",
                FirstName = "John",
                LastName = "Doe",
                DateBirth = new DateTime(1990, 1, 1),
                Email = "john.doe@example.com",
                Company = "ABC Corp",
                AccountStatus = "Active"
            });

            accountRepo.Add(new Account
            {
                Avatar = "avatar2.png",
                FirstName = "Jane",
                LastName = "Smith",
                DateBirth = new DateTime(1992, 5, 12),
                Email = "jane.smith@example.com",
                Company = "XYZ Inc",
                AccountStatus = "Active"
            });

            accountRepo.Add(new Account
            {
                Avatar = "avatar3.png",
                FirstName = "Alice",
                LastName = "Johnson",
                DateBirth = new DateTime(1988, 7, 23),
                Email = "alice.johnson@example.com",
                Company = "Tech Solutions",
                AccountStatus = "Inactive"
            });

            accountRepo.Add(new Account
            {
                Avatar = "avatar4.png",
                FirstName = "Bob",
                LastName = "Brown",
                DateBirth = new DateTime(1995, 3, 15),
                Email = "bob.brown@example.com",
                Company = "Innovate Ltd",
                AccountStatus = "Active"
            });

            accountRepo.Add(new Account
            {
                Avatar = "avatar5.png",
                FirstName = "Carol",
                LastName = "Davis",
                DateBirth = new DateTime(1991, 11, 30),
                Email = "carol.davis@example.com",
                Company = "Creative Co",
                AccountStatus = "Inactive"
            });

            var workspaceRepo = new WorkspaceRepository(connection);

            // Thêm 3 workspace mẫu
            workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "Marketing Team",
                CreatedBy = 1,
                IsPersonal = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "Development Team",
                CreatedBy = 2,
                IsPersonal = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "Personal Workspace",
                CreatedBy = 1,
                IsPersonal = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            var workspaceMemberRepo = new WorkspaceMemberRepository(connection);

            // Thêm dữ liệu mẫu cho WorkspaceMember
            workspaceMemberRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 1,
                AccountId = 1,
                JoinedAt = DateTime.Now,
                MemberStatus = "Active",
                UpdatedAt = DateTime.Now
            });

            workspaceMemberRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 1,
                AccountId = 2,
                JoinedAt = DateTime.Now,
                MemberStatus = "Active",
                UpdatedAt = DateTime.Now
            });

            workspaceMemberRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 2,
                AccountId = 2,
                JoinedAt = DateTime.Now,
                MemberStatus = "Active",
                UpdatedAt = DateTime.Now
            });

            workspaceMemberRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 2,
                AccountId = 4,
                JoinedAt = DateTime.Now,
                MemberStatus = "Active",
                UpdatedAt = DateTime.Now
            });

            workspaceMemberRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 3,
                AccountId = 1,
                JoinedAt = DateTime.Now,
                MemberStatus = "Active",
                UpdatedAt = DateTime.Now
            });
        }
    }
}

using System.Data;
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListRepository;
using MSListsApp.Dapper.Repositories.FavoriteListRepository;
using MSListsApp.Dapper.Repositories.ListTypeRepository;

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
            accountRepo.CreateTable();

            var workspaceRepo = new WorkspaceRepository(connection);
            workspaceRepo.CreateTable();

            var workspaceMemberRepo = new WorkspaceMemberRepository(connection);
            workspaceMemberRepo.CreateTable();

            var listRepo = new ListRepository(connection);
            listRepo.CreateTable();

            var favoriteListRepo = new FavoriteListRepository(connection);
            favoriteListRepo.CreateTable();

            var listTypeRepo = new ListTypeRepository(connection);
            listTypeRepo.CreateTable();

        }

        public static void SeedData(IDbConnection connection)
        {
            SeedAccounts(connection);
            SeedWorkspaces(connection);
            SeedWorkspaceMembers(connection);
            SeedLists(connection);
            SeedFavoriteLists(connection);
            SeedListType(connection);
        }

        private static void SeedAccounts(IDbConnection connection)
        {
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
        }

        private static void SeedWorkspaces(IDbConnection connection)
        {
            var workspaceRepo = new WorkspaceRepository(connection);
            var ws1Id = workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "My List",
                CreatedBy = 1,
                IsPersonal = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            var ws2Id = workspaceRepo.Add(new Workspace
            {
                WorkspaceName = "My List",
                CreatedBy = 2,
                IsPersonal = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        private static void SeedWorkspaceMembers(IDbConnection connection)
        {
            var wmRepo = new WorkspaceMemberRepository(connection);
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 1,
                AccountId = 1,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 1,
                AccountId = 2,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
            wmRepo.Add(new WorkspaceMember
            {
                WorkspaceId = 2,
                AccountId = 2,
                JoinedAt = DateTime.UtcNow,
                MemberStatus = "Active",
                UpdatedAt = DateTime.UtcNow
            });
        }

        private static void SeedLists(IDbConnection connection)
        {
            var listRepo = new ListRepository(connection);
            var list1Id = listRepo.Add(new List
            {
                ListTypeId = 1,
                ListTemplateId = null,
                WorkspaceId = 1,
                ListName = "Project Tasks",
                Icon = "📋",
                Color = "#4CAF50",
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow,
                ListStatus = "Active"
            });
            var list2Id = listRepo.Add(new List
            {
                ListTypeId = 1,
                ListTemplateId = null,
                WorkspaceId = 2,
                ListName = "Shopping List",
                Icon = "🛒",
                Color = "#2196F3",
                CreatedBy = 2,
                CreatedAt = DateTime.UtcNow,
                ListStatus = "Active"
            });
            var list3Id = listRepo.Add(new List
            {
                ListTypeId = 1,
                ListTemplateId = null,
                WorkspaceId = 1,
                ListName = "Project Tasks 2",
                Icon = "📋",
                Color = "#4CAF50",
                CreatedBy = 1,
                CreatedAt = DateTime.UtcNow,
                ListStatus = "Active"
            });
        }
        private static void SeedFavoriteLists(IDbConnection connection)
        {
            var favoriteListRepo = new FavoriteListRepository(connection);
            favoriteListRepo.Add(new FavoriteList
            {
                ListId = 1, // Project Tasks
                AccountId = 1, // John
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });

            favoriteListRepo.Add(new FavoriteList
            {
                ListId = 1, // Project Tasks
                AccountId = 2, // Jane
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            favoriteListRepo.Add(new FavoriteList
            {
                ListId = 2, // Shopping List
                AccountId = 2, // Jane
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            favoriteListRepo.Add(new FavoriteList
            {
                ListId = 3, // Project Tasks 2
                AccountId = 1, // John
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
        }

        private static void SeedListType(IDbConnection connection)
        {
            var listTypeRepo = new ListTypeRepository(connection);
            listTypeRepo.Add(new ListType
            {
                Title = "List",
                Icon = "list.png",
                ListTypeDescription = "A standard list view for tasks or items.",
                HeaderImage = "list-header.jpg"
            });
            listTypeRepo.Add(new ListType
            {
                Title = "Form",
                Icon = "form.png",
                ListTypeDescription = "A form-based interface for data entry.",
                HeaderImage = "form-header.jpg"
            });
            listTypeRepo.Add(new ListType
            {
                Title = "Board",
                Icon = "board.png",
                ListTypeDescription = "A board view for organizing tasks, like Kanban.",
                HeaderImage = "board-header.jpg"
            });
            listTypeRepo.Add(new ListType
            {
                Title = "Gallery",
                Icon = "gallery.png",
                ListTypeDescription = "A gallery view for displaying images or cards.",
                HeaderImage = "gallery-header.jpg"
            });
            listTypeRepo.Add(new ListType
            {
                Title = "Calendar",
                Icon = "calendar.png",
                ListTypeDescription = "A calendar view for scheduling events.",
                HeaderImage = "calendar-header.jpg"
            });
        }
    }
            

}

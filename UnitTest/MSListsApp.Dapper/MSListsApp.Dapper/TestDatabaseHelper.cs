using System.Data;
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.ListRepository;
using MSListsApp.Dapper.Repositories.FavoriteListRepository;
using MSListsApp.Dapper.Repositories.ListTypeRepository;
using Dapper;

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
            var createTablesSql = @"
            CREATE TABLE IF NOT EXISTS Account (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Avatar TEXT,
                FirstName TEXT,
                LastName TEXT,
                DateBirth DATETIME,
                Email TEXT,
                Company TEXT,
                AccountStatus TEXT,
                AccountPassword TEXT
            );

           CREATE TABLE IF NOT EXISTS Workspace (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                WorkspaceName TEXT,
                CreatedBy INTEGER,
                WorkspaceDescription TEXT,
                IsPersonal BIT,
                CreatedAt DATETIME,
                UpdatedAt DATETIME
            );

            CREATE TABLE IF NOT EXISTS WorkspaceMember (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                WorkspaceId INTEGER,
                AccountId INTEGER,
                Role TEXT,
                JoinedAt DATETIME,
                MemberStatus TEXT,
                UpdatedAt DATETIME
            );

            CREATE TABLE IF NOT EXISTS List (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ListName TEXT,
                Icon TEXT,
                Color TEXT,
                WorkspaceId INTEGER,
                ListStatus TEXT,
                ListTypeId INTEGER,
                ListTemplateId INTEGER,
                CreatedBy INTEGER,
                CreatedAt DATETIME
            );

            CREATE TABLE IF NOT EXISTS FavoriteList (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                ListId INTEGER,
                AccountId INTEGER,
                CreatedAt DATETIME,
                UpdatedAt DATETIME
            );

            CREATE TABLE IF NOT EXISTS ListType (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT,
                Icon TEXT,
                ListTypeDescription TEXT,
                HeaderImage TEXT
            );
        ";

            connection.Execute(createTablesSql);
        }

        public static void SeedData(IDbConnection connection)
        {
            var now = DateTime.UtcNow;

            var insertSql = @"
            -- Accounts
            INSERT INTO Account (Avatar, FirstName, LastName, Email, Company, AccountStatus, AccountPassword)
            VALUES 
                ('avatar1.png', 'John', 'Doe', 'john@example.com', 'CompanyA', 'Active', '123456'),
                ('avatar2.png', 'Jane', 'Smith', 'jane@example.com', 'CompanyB', 'Active', 'abcdef');

            -- Workspaces
            INSERT INTO Workspace (WorkspaceName, CreatedBy, IsPersonal, CreatedAt, UpdatedAt)
            VALUES 
                ('My List', 1, 1, @Now, @Now),
                ('My List', 2, 1, @Now, @Now);

            -- Workspace Members
            INSERT INTO WorkspaceMember (WorkspaceId, AccountId, JoinedAt, MemberStatus, UpdatedAt)
            VALUES 
                (1, 1, @Now, 'Active', @Now),
                (1, 2, @Now, 'Active', @Now),
                (2, 2, @Now, 'Active', @Now);

            -- List Types
            INSERT INTO ListType (Title, Icon, ListTypeDescription, HeaderImage)
            VALUES
                ('List', 'list.png', 'A standard list view for tasks or items.', 'list-header.jpg'),
                ('Form', 'form.png', 'A form-based interface for data entry.', 'form-header.jpg'),
                ('Board', 'board.png', 'A board view for organizing tasks, like Kanban.', 'board-header.jpg'),
                ('Gallery', 'gallery.png', 'A gallery view for displaying images or cards.', 'gallery-header.jpg'),
                ('Calendar', 'calendar.png', 'A calendar view for scheduling events.', 'calendar-header.jpg');

            -- Lists
            INSERT INTO List (ListTypeId, ListTemplateId, WorkspaceId, ListName, Icon, Color, CreatedBy, CreatedAt, ListStatus)
            VALUES
                (1, NULL, 1, 'Project Tasks', '📋', '#4CAF50', 1, @Now, 'Active'),
                (1, NULL, 2, 'Shopping List', '🛒', '#2196F3', 2, @Now, 'Active'),
                (1, NULL, 1, 'Project Tasks 2', '📋', '#4CAF50', 1, @Now, 'Active');

            -- Favorite Lists
            INSERT INTO FavoriteList (ListId, AccountId, CreatedAt, UpdatedAt)
            VALUES
                (1, 1, @Now, @Now),
                (1, 2, @Now, @Now),
                (2, 2, @Now, @Now),
                (3, 1, @Now, @Now);
        ";

            connection.Execute(insertSql, new { Now = now });
        }
    }
}
            



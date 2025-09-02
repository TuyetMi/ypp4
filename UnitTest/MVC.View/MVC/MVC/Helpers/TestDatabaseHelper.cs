
using System.Data;
using Microsoft.Data.Sqlite;

namespace MVC.Helpers
{
    public class TestDatabaseHelper
    {
        private static SqliteConnection? _connection;

        public static void InitDatabase()
        {
            _connection = new SqliteConnection("Data Source=:memory:;");
            _connection.Open();

            CreateTable(_connection);
            SeedData(_connection);
        }

        private static void CreateTable(IDbConnection connection)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE Account (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Avatar TEXT NOT NULL,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    DateBirth DATETIME,
                    Email TEXT NOT NULL,
                    Company TEXT NOT NULL,
                    Status INTEGER NOT NULL,
                    AccountPassword TEXT NOT NULL
                );";
            cmd.ExecuteNonQuery();

            // Tạo bảng Workspace
            cmd.CommandText = @"
                CREATE TABLE Workspace (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    WorkspaceName TEXT NOT NULL,
                    CreatedBy INTEGER NOT NULL,
                    IsPersonal INTEGER NOT NULL DEFAULT 0,
                    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    UpdatedAt DATETIME,
                    FOREIGN KEY (CreatedBy) REFERENCES Account(Id)
                );";
            cmd.ExecuteNonQuery();

            // Tạo bảng WorkspaceMember
            cmd.CommandText = @"
                CREATE TABLE WorkspaceMember (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    WorkspaceId INTEGER NOT NULL,
                    AccountId INTEGER NOT NULL,
                    JoinedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
                    MemberStatus INTEGER NOT NULL,
                    UpdatedAt DATETIME,
                    FOREIGN KEY (WorkspaceId) REFERENCES Workspace(Id),
                    FOREIGN KEY (AccountId) REFERENCES Account(Id)
                );";
            cmd.ExecuteNonQuery();

            // ListType
            cmd.CommandText = @"
                CREATE TABLE ListType (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Icon TEXT,
                    ListTypeDescription TEXT,
                    HeaderImage TEXT
                );";
            cmd.ExecuteNonQuery();

            // List
            cmd.CommandText = @"
                CREATE TABLE List (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ListTypeId INTEGER NOT NULL,
                    ListTemplateId INTEGER,
                    WorkspaceID INTEGER,
                    ListName TEXT NOT NULL,
                    Icon TEXT,
                    Color TEXT,
                    CreatedBy INTEGER NOT NULL,
                    CreatedAt DATETIME,
                    ListStatus TEXT DEFAULT 'Active',
                    FOREIGN KEY (ListTypeId) REFERENCES ListType(Id),
                    FOREIGN KEY (WorkspaceID) REFERENCES Workspace(Id),
                    FOREIGN KEY (CreatedBy) REFERENCES Account(Id)
                );";
            cmd.ExecuteNonQuery();

            // RecentList
            cmd.CommandText = @"
                CREATE TABLE RecentList (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    AccountId INTEGER NOT NULL,
                    ListId INTEGER NOT NULL,
                    LastAccessedAt DATETIME NOT NULL,
                    UNIQUE(AccountId, ListId),
                    FOREIGN KEY (AccountId) REFERENCES Account(Id),
                    FOREIGN KEY (ListId) REFERENCES List(Id)
                );";
            cmd.ExecuteNonQuery();

            // FavoriteList
            cmd.CommandText = @"
                CREATE TABLE FavoriteList (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ListId INTEGER NOT NULL,
                    AccountId INTEGER NOT NULL,
                    CreatedAt DATETIME,
                    UpdatedAt DATETIME,
                    FOREIGN KEY (ListId) REFERENCES List(Id),
                    FOREIGN KEY (AccountId) REFERENCES Account(Id)
                );";
            cmd.ExecuteNonQuery();

        }

        private static void SeedData(IDbConnection connection)
        {
            ExecuteNonQuery(connection, @"
                INSERT INTO Account (Avatar, FirstName, LastName, DateBirth, Email, Company, Status, AccountPassword)
                VALUES
                ('avatar1.png', 'John', 'Doe', '1990-01-01', 'john@example.com', 'Company A', 1, 'password123'),
                ('avatar2.png', 'Jane', 'Smith', '1992-05-10', 'jane@example.com', 'Company B', 2, 'password456'),
                ('avatar3.png', 'Alice', 'Johnson', NULL, 'alice@example.com', 'Company C', 1, 'alice789');
            ");

            // Seed Workspace cá nhân (tên My lists)
            ExecuteNonQuery(connection, @"
                INSERT INTO Workspace (WorkspaceName, CreatedBy, IsPersonal, CreatedAt)
                VALUES
                ('My lists', 1, 1, '2025-08-19 10:00:00'),
                ('My lists', 2, 1, '2025-08-19 10:00:00'),
                ('My lists', 3, 1, '2025-08-19 10:00:00');
            ");

            // Seed WorkspaceMember (Owner)
            ExecuteNonQuery(connection, @"
                INSERT INTO WorkspaceMember (WorkspaceId, AccountId, MemberStatus, JoinedAt)
                VALUES
                (1, 1, 1, '2025-08-19 10:00:00'),
                (2, 2, 1, '2025-08-19 10:00:00'),
                (3, 3, 1, '2025-08-19 10:00:00');
            ");

            // ListType
            ExecuteNonQuery(connection, @"
                INSERT INTO ListType (Title, Icon, ListTypeDescription, HeaderImage)
                VALUES 
                ('List', 'list_icon.png', 'Standard list for managing items', 'header_list.png'),
                ('Form', 'form_icon.png', 'Customizable form for data entry', 'header_form.png'),
                ('Gallery', 'gallery_icon.png', 'Visual gallery to display items as cards', 'header_gallery.png'),
                ('Calendar', 'calendar_icon.png', 'Calendar view for scheduling and deadlines', 'header_calendar.png'),
                ('Board', 'board_icon.png', 'Kanban board for task management', 'header_board.png');
            ");

            // List
            ExecuteNonQuery(connection, @"
                INSERT INTO List (ListTypeId, ListTemplateId, WorkspaceID, ListName, Icon, Color, CreatedBy, CreatedAt, ListStatus)
                VALUES
                (1, NULL, 1, 'Project Tasks', '📋', 'Blue', 1, '2025-08-20 09:00:00', 'Active'),
                (2, NULL, 1, 'Employee Form', '📝', 'Green', 1, '2025-08-20 10:00:00', 'Active'),
                (3, NULL, 2, 'Marketing Gallery', '🖼️', 'Purple', 2, '2025-08-21 14:30:00', 'Active'),
                (4, NULL, 3, 'Event Calendar', '📅', 'Red', 3, '2025-08-22 08:15:00', 'Active'),
                (5, NULL, 3, 'Sprint Board', '🗂️', 'Orange', 3, '2025-08-22 09:45:00', 'Archived');
            ");

            // RecentList
            ExecuteNonQuery(connection, @"
                INSERT INTO RecentList (AccountId, ListId, LastAccessedAt)
                VALUES
                (1, 1, '2025-08-25 08:00:00'),
                (1, 2, '2025-08-25 09:30:00'),
                (2, 3, '2025-08-26 10:00:00'),
                (3, 4, '2025-08-26 11:15:00'),
                (3, 5, '2025-08-26 15:20:00');
            ");

            // FavoriteList
            ExecuteNonQuery(connection, @"
                INSERT INTO FavoriteList (ListId, AccountId, CreatedAt, UpdatedAt)
                VALUES
                (1, 1, '2025-08-25 08:10:00', '2025-08-25 08:10:00'),
                (3, 2, '2025-08-26 10:15:00', '2025-08-26 10:15:00'),
                (4, 3, '2025-08-26 11:30:00', '2025-08-26 11:30:00');
            ");
        }

        private static void ExecuteNonQuery(IDbConnection connection, string sql)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public static SqliteConnection GetConnection()
        {
            if (_connection == null)
                throw new InvalidOperationException("Database not initialized. Call InitDatabase() first.");
            return _connection;
        }

        public static void CloseDatabase()
        {
            _connection?.Close();
            _connection = null;
        }
    }
}
    

using System.Data;
using Microsoft.Data.Sqlite;

namespace MVC.Data
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
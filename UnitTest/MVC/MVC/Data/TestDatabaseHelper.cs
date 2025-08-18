
using System.Data;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using MVC.Models;

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
        }

        private static void SeedData(IDbConnection connection)
        {
            var sql = @"
                INSERT INTO Account (Avatar, FirstName, LastName, DateBirth, Email, Company, Status, AccountPassword)
                VALUES
                ('avatar1.png', 'John', 'Doe', '1990-01-01', 'john@example.com', 'Company A', 1, 'password123'),
                ('avatar2.png', 'Jane', 'Smith', '1992-05-10', 'jane@example.com', 'Company B', 2, 'password456'),
                ('avatar3.png', 'Alice', 'Johnson', NULL, 'alice@example.com', 'Company C', 1, 'alice789');
            ";

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
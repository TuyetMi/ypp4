using System;
using System.Collections.Generic;
using System.Data;


using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.Repositories;
using MSListsApp.Dapper.Repositories.AccountRepository;
using MSListsApp.Dapper.Repositories.WorkspaceRepository;
using SQLitePCL;

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

        }
    }
}

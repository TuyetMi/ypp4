using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;
using MSListsApp.Dapper.DTOs;
using MSListsApp.Dapper.Models;
using MSListsApp.Dapper.Repositories.WorkspaceMemberRepository;
using MSListsApp.Dapper.Services.WorkspaceMemberService;


namespace MSListsApp.Dapper.Tests
{
    [TestClass] 
    public class WorkspaceMemberTests
    {
        private SqliteConnection _connection;
        private IWorkspaceMemberRepository _repository;
        private IWorkspaceMemberService _service;

        [TestInitialize]
        public void Setup()
        {
            _connection = TestDatabaseHelper.CreateInMemoryDatabase();
            TestDatabaseHelper.CreateAllTables(_connection);
            TestDatabaseHelper.SeedData(_connection);

            _repository = new WorkspaceMemberRepository(_connection);
            _service = new WorkspaceMemberService(_repository);
        }
        [TestCleanup]
        public void Cleanup()
        {
            // Đóng kết nối sau mỗi test
            _connection.Close();
            _connection.Dispose();
        }
 

    }
}

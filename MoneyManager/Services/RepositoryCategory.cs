using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public interface IRepositoryCategory
    {
        Task Create(Category category);
        Task Delete(int id);
        Task<IEnumerable<Category>> Get(int UserId, OperationType TransactionTypeId);
        Task<IEnumerable<Category>> Get(int UserId);
        Task<Category> GetForId(int id, int UserId);
        Task Update(Category category);
    }
    public class RepositoryCategory : IRepositoryCategory
    {
        private readonly string connectionString;

        public RepositoryCategory(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Category category)
        {
            var TransactionTypeId = 1;

            if(category.TransactionTypeId.ToString() == "Spending")
            {
                TransactionTypeId = 2;
            }

            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                                        INSERT INTO Categories (Name, TransactionTypeId, UserId)
                                        Values (@Name, @TransactionTypeId, @UserId);
                                        SELECT SCOPE_IDENTITY();
                                        ", new { category.Name, category.UserId, TransactionTypeId });

            category.Id = id;
        }

        public async Task<IEnumerable<Category>> Get(int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>(
                "SELECT * FROM Categories WHERE UserId = @UserId", new { UserId });
        }

        public async Task<IEnumerable<Category>> Get(int UserId, OperationType TransactionType)
        {
            int TransactionTypeId;
                    
            if(TransactionType.ToString() == "Ingreso")
            {
                TransactionTypeId = 1;
            }
            else
            {
                TransactionTypeId = 2;
            }

            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Category>(
                @"SELECT * 
            FROM Categories 
            WHERE UserId = @UserId AND TransactionTypeId = @TransactionTypeId",
                new { UserId, TransactionTypeId });
        }

        public async Task<Category> GetForId(int id, int UserId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Category>(
                        @"Select * FROM Categories WHERE Id = @Id AND UserId = @UserId",
                        new { id, UserId });
        }

        public async Task Update(Category category)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categories 
                    SET Name = @Name, TransactionTypeId = @TransactionTypeId
                    WHERE Id = @Id", category);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Categories WHERE Id = @Id", new { id });
        }
    }
}

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
    public interface IRepositoryCounts
    {
        Task Create(Count count);
        Task Delete(int id);
        Task<Count> GetForID(int id, int userId);
        Task<IEnumerable<Count>> Search(int userId);
        Task<IEnumerable<Count>> SearchForCondition(int userId, bool condition);
        Task Update(CreateCountViewModel count);
    }
    public class CountsRepository : IRepositoryCounts
    {
        public readonly string _connectionString;
        public CountsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Count count)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
               await connection.QuerySingleAsync(@"INSERT 
                INTO Counts(Name, CountTypeId, Description, Balance, Condition)
                values(@name, @countTypeId, @description, @balance, @condition);
                Select SCOPE_IDENTITY();", count);
            }
        }

        public async Task<IEnumerable<Count>> Search(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Count>(@"SELECT  Counts.Id,
                                    Counts.Name, Balance,Condition,
                                    ct.Name as CountType
                                    FROM Counts 
                                    inner Join CountsType  ct
                                    on ct.id = Counts.CountTypeId
                                    where ct.UserId = @userId
                                    order by ct.OrderType
                                    ", new { userId});
            }
        }
        public async Task<IEnumerable<Count>> SearchForCondition(int userId, bool condition)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<Count>(@"SELECT  Counts.Id,
                                    Counts.Name, Balance,Condition,
                                    ct.Name as CountType
                                    FROM Counts 
                                    inner Join CountsType  ct
                                    on ct.id = Counts.CountTypeId
                                    where ct.UserId = @userId and Condition = @condition
                                    order by ct.OrderType
                                    ", new { userId, condition});
            }
        }

        public async Task<Count> GetForID(int id, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryFirstOrDefaultAsync<Count>(@"SELECT Counts.Id, Counts.Name,
               Counts.Condition,
               Balance, Description, tc.Id
                FROM Counts
                INNER JOIN CountsType tc
                ON tc.Id = Counts.CountTypeId
                WHERE tc.UserId = @UserId AND Counts.Id = @Id", 
                new { userId, id });
                                   
            }
        }

        public async Task Update(CreateCountViewModel count)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"UPDATE Counts
                                    SET Name = @Name, Balance = @balance, Description = @Description,
                                    CountTypeId = @CountTypeId
                                    WHERE Id = @Id;", count);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(@"Delete from Counts
                                    WHERE Id = @Id;", new { id });
        }
    }
}

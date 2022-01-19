using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace MoneyManager.Services
{
    public interface ICountTypeRepository
    {
        Task Create(CountType countType);
        Task Delete(int id);
        Task<bool> ExistCountType(CountType countType, string name);
        Task<IEnumerable<CountType>> Get(int userId);
        Task<CountType> GetForId(int id, int userId);
        Task Order(IEnumerable<CountType> countTypesOrder);
        Task Update(CountType count);
    }

    public class CountTypeRepository : ICountTypeRepository
    {
        private readonly string _connectionString;
        public CountTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(CountType countType)
        {
            if (!await ExistCountType(countType))
            {
                using(var connection = new SqlConnection(_connectionString))
                {
                    var id = await connection.QuerySingleAsync<int>("CountsType_insert", 
                        new {
                         userId = countType.UserId,
                         Name = countType.Name
                        }, commandType: CommandType.StoredProcedure);

                    countType.Id =  id;
                }
            }
            else
            {
                throw new Exception($"El Nombre: {countType.Name} ya existe!");
            }

        }
        public async Task<bool> ExistCountType(CountType countType = null, string name = null )
        {

            using (var connection = new SqlConnection(_connectionString))
            {
              if(countType != null)
              {
                    var exist = await connection.QueryFirstOrDefaultAsync<int>(
                        $@"SELECT 1
                          FROM [MoneyManager].[dbo].[CountsType]
                          where Name = @name and UserId = @userId ", countType);

                    return exist == 1;
              }
              else if(countType == null && name != null)
              {
                    var userId = 1; 
                    var exist = await connection.QueryFirstOrDefaultAsync<int>(
                    $@"SELECT 1
                          FROM [MoneyManager].[dbo].[CountsType]
                          where Name = @name and UserId = @userId ", new { name, userId });

                    return exist == 1;
              }
            }
            return true;
          
        }
        public async Task<IEnumerable<CountType>> Get(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<CountType>(@$"
                SELECT ID,NAME, ORDERTYPE FROM 
                CountsType WHERE UserId = @userId
                Order By  OrderType",new { userId });
            }
        }
        public async Task Update(CountType count)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(@"      
                    UPDATE CountsType 
                    Set Name = @name,
                    orderType = @orderType
                    where Id = @id", count);
            }
        }
        public async Task<CountType> GetForId(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<CountType>(@" 
                    select Id, OrderType, Name from CountsType
                    where Id = @Id and UserId = @userId", new {id, userId});   

        }
        public async Task Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync("Delete CountsType where Id = @id", new {id}) ;
            }
        }
        public async Task Order(IEnumerable<CountType> countTypesOrder)
        {      
            using (var connection = new SqlConnection(_connectionString))
            {
                foreach(var ctOrder in countTypesOrder)
                {
                   await connection.ExecuteAsync(@"UPDATE CountsType SET OrderType = @OrderType
                        where Id = @id", new { ctOrder.OrderType, ctOrder.Id });
                }
            }
        }

    }
}

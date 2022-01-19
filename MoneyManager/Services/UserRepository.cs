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
    public interface IUserRepository
    {
        public Task<int> Create(User user);
        Task<User> SearchUser(string EmailNormalize);

    }

    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task<int> Create(User user)
        {
            using var connection = new SqlConnection(_connectionString);
           // user.EmailNormalize = user.Email.ToUpper();
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Users
                                                           (Email
                                                           ,EmailNormalize
                                                           ,PasswordHsh)
                                                            VALUES
                                                           (@Email
                                                           ,@EmailNormalize
                                                           ,@PasswordHsh)
                                                           Select Scope_Identity();", user);

            return id;

        }
        public async Task<User> SearchUser(string EmailNormalize)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<User>(@"SELECT * 
            FROM USERS WHERE EmailNormalize = @emailNormalize ",new { EmailNormalize });


        }

    }
}

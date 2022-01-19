﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public interface ITransactionRepository
    {
        Task Create(Transaction transaccion);
        Task Delete(int id);
        Task<Transaction> GetForId(int id, int userId);
        Task<IEnumerable<Transaction>> GetTransactionsForCount(GetTransactionForCount model);
        Task<IEnumerable<Transaction>> GetTransactionsForExport(GetTransactionForCount model);
        Task<IEnumerable<Transaction>> GetTransactionsForChart(bool condition, int userId);
        Task Update(Transaction transaction, decimal LastMount, int LastCountId);
    }
    public class TransactionRepository : ITransactionRepository
    {
        private readonly string connectionString;

        public TransactionRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task Create(Transaction transaccion)
        {
            int TransactionTypeId;
            using var connection = new SqlConnection(connectionString);
            var date = transaccion.DateTransaction;
            if (transaccion.TransactionType == OperationType.Ingreso) 
            {
                TransactionTypeId = 1;
            }
            else
            {
                TransactionTypeId = 2;
            }
            var id = await connection.QuerySingleAsync<int>("InsertTransaction",
                new
                {
                    transaccion.UserId,
                    date,
                    transaccion.Mount,
                    transaccion.Note,
                    transaccion.CategoryId,
                    transaccion.CountId,
                    TransactionTypeId,
                    transaccion.Condition
                },  
                commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }

        public async Task Update(Transaction transaction, decimal LastMount,
            int LastCountId)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("UpdateTransaction",
                new
                {
                    transaction.Id,
                    transaction.DateTransaction,
                    transaction.Mount,
                    transaction.CategoryId,
                    transaction.CountId,
                    transaction.Note,
                    LastMount,
                    LastCountId
                }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Transaction> GetForId(int id, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transaction>(
                @"SELECT Transaction.*, cat.TransactionTypeId
                FROM Transaction
                INNER JOIN Categories cat
                ON cat.Id = Transaction.CategoryId
                WHERE Transaction.Id = @Id AND Transaction.UserId = @UserId",
                new { id, userId });
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DeleteTransaction",
                new { id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsForCount(GetTransactionForCount model)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(@"select t.Mount,
            t.DateTransaction, c.Name as category, c.TransactionTypeId FROM [Transaction] T
            inner join Categories C ON c.Id = t.CategoryId
            inner join Counts cou on cou.Id = t.CountId
            where t.UserId = @userId and t.CountId = @countId
            and DateTransaction between @startdate and @endDate", model);
        }  
        public async Task<IEnumerable<Transaction>> GetTransactionsForExport(GetTransactionForCount model)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(@"select t.Mount,
            t.DateTransaction, c.Name as category, c.TransactionTypeId, cou.Name, t.Condition FROM [Transaction] T
            inner join Categories C ON c.Id = t.CategoryId
            inner join Counts cou on cou.Id = t.CountId
            where t.UserId = @UserId
            and DateTransaction between @StartDate and @endDate
			order by t.DateTransaction", model);
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsForChart(bool condition, int userId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transaction>(@"select t.Mount,
            c.Name as category, cou.Name, t.Condition, t.DateTransaction FROM [Transaction] T
            inner join Categories C ON c.Id = t.CategoryId
            inner join Counts cou on cou.Id = t.CountId
              where t.UserId = @userId and t.Condition = @condition
            ", new { userId, condition});
        }

    }
}
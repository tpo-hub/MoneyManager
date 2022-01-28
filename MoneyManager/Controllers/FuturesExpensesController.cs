using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace MoneyManager.Controllers
{
    public class FuturesExpensesController : Controller
    {
        private readonly string connectionString;
        private readonly ITransactionRepository transactionRepository;
        private readonly IRepositoryCounts counts;
        private readonly IRepositoryCategory categories;

        public FuturesExpensesController(IConfiguration configuration, 
            ITransactionRepository transactionRepository,
             IRepositoryCounts counts, IRepositoryCategory categories)
           
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            this.transactionRepository = transactionRepository;
            this.counts = counts;
            this.categories = categories;
        }
        // GET: FuturesExpensesController
        public async Task<ActionResult> Index()
        {
            using var connection = new SqlConnection(connectionString);
            var model = await connection.QueryAsync<FutureExpenses>(@"SELECT [Id]
                                                          ,[Name]
                                                          ,[Description]
                                                          ,[Date]
                                                          ,[CountId]
                                                          ,[CategoryId]
                                                          ,[Mount]
                        FROM [MoneyManager].[dbo].[FutureExpenses]");
            return View(model);
        }


        public async Task<ActionResult> Create()
        {

            ViewBag.Categories = await GetCategories(1, OperationType.Gasto);
            ViewBag.Counts = await GetCounts(1, false);   
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(FutureExpenses fExpenses)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"
                INSERT INTO [dbo].[FutureExpenses]
               ([Name]
               ,[Description]
               ,[Date]
               ,[CountId]
               ,[CategoryId]
               ,[Mount])
                 VALUES
               (@Name,@Description,@Date,@CountId,@CategoryId,@Mount)", fExpenses);
            return RedirectToAction("Index");
        }

        public async Task<JsonResult> Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            var fExpnses = await connection.QueryFirstOrDefaultAsync<FutureExpenses>(@"SELECT [Id]
                                                          ,[Name]
                                                          ,[Description]
                                                          ,[Date]
                                                          ,[CountId]
                                                          ,[CategoryId]
                                                          ,[Mount]
                        FROM [FutureExpenses] Where ID = @id", new { Id });

            var result = new { Success = "True", Message = "El pendiente ahora es una transaccion!" };
            if (fExpnses != null)
            {
                await transactionRepository.Create(new Transaction
                {
                    CategoryId = fExpnses.CategoryId,
                    Condition = false,
                    CountId = fExpnses.CountId,
                    Note = fExpnses.Description,
                    Mount = fExpnses.Mount,
                    DateTransaction = fExpnses.Date,
                    TransactionType = OperationType.Gasto,
                    UserId = 1
                });

                await connection.ExecuteAsync(@"Delete from FutureExpenses
                                        WHERE Id = @Id;", new { Id });
                return Json(result);
            }
            result = new { Success = "False", Message = "No se completo la transaccion" };
            return Json(result);

        }


        private async Task<IEnumerable<SelectListItem>> GetCounts(int userId, bool condition)
        {
            var Usercounts = await counts.SearchForCondition(userId, condition);
            return Usercounts.Select(x => new SelectListItem(
                $"{x.Name} - {x.CountType} ",
                x.Id.ToString())).ToList();

        }
        private async Task<IEnumerable<SelectListItem>> GetCategories(int userId, OperationType transaction)
        {
            var Usercats = await categories.Get(userId, transaction);
            return Usercats.Select(x => new SelectListItem
            (
                x.Name,
                x.Id.ToString()
            ));
        }



    }
}

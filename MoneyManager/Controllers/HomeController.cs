using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly ITransactionRepository TransactionRepository;
        public HomeController(IUserService userService, ITransactionRepository transactionRepository)
        {
            this.userService = userService;
            this.TransactionRepository = transactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUser();
             var transactionsNegative = await TransactionRepository.GetTransactionsForChart(false, userId);
             var transactionsPositive = await TransactionRepository.GetTransactionsForChart(true, userId);

            ViewBag.pasives = transactionsNegative.Select(x => new TransacitonChart()
            {
                Category = x.Category,
                Condition = x.Condition,
                Mount = x.Mount,
                Name = x.Count,
                Date = x.DateTransaction.ToString("yyyy-MM-dd")
            });

            ViewBag.actives = transactionsPositive.Select(x => new TransacitonChart()
            {
                Category = x.Category,
                Condition = x.Condition,
                Mount = x.Mount,
                Name = x.Count,
                Date = x.DateTransaction.ToString("yyyy-MM-dd")
            });
            return View();
        }

        public IActionResult NoEncontrado()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<JsonResult> GetCalendar(DateTime start, DateTime end)
        {
            var userId = userService.GetUser();
            var transactionObj = new GetTransactionForCount()
            {
                UserId = userId,
                StartDate = start,
                endDate = end
            };

            var transaction = await TransactionRepository.GetTransactionsForExport(transactionObj);
            var forCalendar = transaction.Select(x => new EventCalendar() { 
                Title = x.Mount.ToString("N"),
                Start = x.DateTransaction.ToString("yyyy-MM-dd"),
                End = x.DateTransaction.ToString("yyyy-MM-dd"),
                Color = (x.Condition == false) ? "Red" : null
            });
            return Json(forCalendar);
        }

    }
}

using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class CountsController : Controller
    {
        private readonly ICountTypeRepository countTypeRepository;
        private readonly IUserService userService;
        private readonly IRepositoryCounts repositoryCounts;
        private readonly IMapper mapper;
        private readonly ITransactionRepository transactionRepository;


        public CountsController(ICountTypeRepository countTypeRepository, IUserService userService,
            IRepositoryCounts repositoryCounts, IMapper mapper, ITransactionRepository transactionRepository)
        {
            this.countTypeRepository = countTypeRepository;
            this.userService = userService;
            this.repositoryCounts = repositoryCounts;
            this.mapper = mapper;
            this.transactionRepository = transactionRepository;
        }
        public async Task<ActionResult> Index()
        {
            var userId = userService.GetUser();
            var countsWithCT = await repositoryCounts.Search(userId);
            var model = countsWithCT.GroupBy(x => x.CountType).Select(group => new IndexCountsViewModel
            {
                CountType = group.Key,
                Counts = group.AsEnumerable(),
            }).ToList();
            return View(model);
        }

        public async Task<ActionResult> Create()
        {
            var userId = userService.GetUser();
            var countsType = await countTypeRepository.Get(userId);
            var model = new CreateCountViewModel();
            model.CountsType = await GetCountType(userId);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateCountViewModel count)
        {
            var userId = userService.GetUser();
            var countTypeIsValid = await countTypeRepository.GetForId(count.CountTypeId, userId);
            if (countTypeIsValid == null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (!ModelState.IsValid)
            {
                count.CountsType = await GetCountType(userId);
                return View(count);
            }
            await repositoryCounts.Create(count);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult<Count>> Edit(int id)
        {
            var userId = userService.GetUser();
            var count = await repositoryCounts.GetForID(id, userId);
            var model = mapper.Map<CreateCountViewModel>(count);
            model.CountsType = await GetCountType(userId);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CreateCountViewModel count)
        {
            var userId = userService.GetUser();
            var countForEdit = await repositoryCounts.GetForID(count.Id, userId);

            if (countForEdit is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            var countType = await countTypeRepository.GetForId(count.CountTypeId, userId);

            if (countType is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            await repositoryCounts.Update(count);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult<Count>> Delete(int id)
        {
            var userId = userService.GetUser();
            var count = await repositoryCounts.GetForID(id, userId);
            var model = mapper.Map<CreateCountViewModel>(count);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(CreateCountViewModel count)
        {
            try
            {
                var userId = userService.GetUser();
                var countForDelete = await repositoryCounts.GetForID(count.Id, userId);

                if (countForDelete is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                await repositoryCounts.Delete(count.Id);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                return View("Error");
            }

        }
        public async Task<IActionResult> Details(int id, int mounth, int year)
        {
            var userId = userService.GetUser();
            var count = await repositoryCounts.GetForID(id, userId);

            DateTime StartDate;
            DateTime EndDate;

            if (mounth == 0 || mounth > 12 || year < 2000)
            {
                var today = DateTime.Today;
                StartDate = new DateTime(today.Year, today.Month, 1);
            }
            else
            {
                StartDate = new DateTime(year, mounth, 1);
            }
            EndDate = StartDate.AddMonths(1).AddDays(-1);

            var getTransactionCounts = new GetTransactionForCount()
            {
                CountId = id,
                UserId = userId,
                StartDate = StartDate,
                endDate = EndDate
            };
            var transactions = await transactionRepository.GetTransactionsForCount(getTransactionCounts);

            var model = new ReportTransactionDetails();
            var transactionsForDate = transactions.OrderByDescending(x => x.DateTransaction)
                .GroupBy(x => x.DateTransaction)
                .Select(x => new ReportTransactionDetails.TransactionsForDate()
                {
                    DateTransaction = x.Key,
                    Transactions = x.AsEnumerable()
                });

            model.TransactionGroup = transactionsForDate;
            model.StartDate = StartDate;
            model.EndDate = EndDate;

            ViewBag.CountCondition = count.Condition;
            ViewBag.CountName = count.Name;
            ViewBag.LastMouth = StartDate.AddMonths(-1).Month;
            ViewBag.LastYear = StartDate.AddMonths(-1).Year;
            ViewBag.NextMouth = StartDate.AddMonths(1).Month;
            ViewBag.NextYear = StartDate.AddMonths(1).Year;
            return View(model);
        }

        public async Task<FileResult> ExportToExcel(int mounth, int year)
        {
            var userId = userService.GetUser();
            var startDate = new DateTime(year, mounth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var getTransactionCounts = new GetTransactionForCount()
            {
                CountId = 0,
                UserId = userId,
                StartDate = startDate,
                endDate = endDate
            };
            var counts = await transactionRepository.GetTransactionsForExport(getTransactionCounts);

            var nameFile = @$"BALANCE DE CUENTAS {DateTime.Today}";
            return GenerateExcel(nameFile, counts);
        }


        #region PrivateMethods

        private FileResult GenerateExcel(string name, IEnumerable<Transaction> transactions)
        {
            DataTable dataTable = new DataTable("Transacciones");
            dataTable.Columns.AddRange(new DataColumn[] { 
                new DataColumn("Fecha"),
                new DataColumn("Cuenta"),
                new DataColumn("Categoria"),
                new DataColumn("Nota"),
                new DataColumn("Monto"),
                new DataColumn("Ingreso o gasto")
            });

            foreach (var transaction in transactions)
            {
                string condition = "";
                if (transaction.Condition)
                {
                    condition = "Ingreso";
                }
                else
                {
                    condition = "Gasto";
                }

                dataTable.Rows.Add(transaction.DateTransaction.ToString("yyyy:MM:dd"), transaction.Count,
                    transaction.Category, transaction.Note, transaction.Mount,condition);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        name);
                }
            }

        }
        private async Task<IEnumerable<SelectListItem>> GetCountType(int userId)
        {
            var countsType = await countTypeRepository.Get(userId);    
            return countsType.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
        }
        #endregion




    }
}

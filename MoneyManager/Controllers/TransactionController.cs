using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class TransactionController : Controller
    {
        private readonly IUserService userService;
        private readonly ITransactionRepository transactionRepository;
        private readonly IMapper mapper;
        private readonly IRepositoryCounts counts;
        private readonly IRepositoryCategory categories;

        public TransactionController(IUserService userService, 
            ITransactionRepository transactionRepository, IMapper mapper,
            IRepositoryCounts counts, IRepositoryCategory categories)
        {
            this.userService = userService;
            this.transactionRepository = transactionRepository;
            this.mapper = mapper;
            this.counts = counts;
            this.categories = categories;
        }
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Create()
        {
            var userId = userService.GetUser();
            var model = new CreateTransactionViewModel();
            model.Categories = await GetCategories(userId, model.TransactionType);
            model.Counts = await GetCounts(userId, model.Condition);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateTransactionViewModel transactionVm)
        {
            var userId = userService.GetUser();
            if (!ModelState.IsValid)
            {
                return View(transactionVm);
            }
            if (transactionVm.TransactionType == OperationType.Gasto)
            {
                transactionVm.Condition = false;
            }
            transactionVm.UserId = userId;
            var transaction = mapper.Map<Transaction>(transactionVm);

            await transactionRepository.Create(transaction);

            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id)
        {
            var userId = userService.GetUser();          
            var transaction = await transactionRepository.GetForId(id,userId);
            var operationType = OperationType.Gasto;
            if(transaction.TransactionType != operationType)
            {
                operationType = OperationType.Ingreso;
            }

            var transactionVm = new CreateTransactionViewModel()
            {
                UserId = userId,
                DateTransaction = transaction.DateTransaction,
                Categories = await GetCategories(userId, operationType),
                TransactionType = transaction.TransactionType,
                Category = transaction.Category,
                CategoryId = transaction.CategoryId,
                Condition = transaction.Condition,
                Count = transaction.Count,
                CountId = transaction.CountId,
                Counts = await GetCounts(userId, transaction.Condition),
                Mount = transaction.Mount,
                Id = transaction.Id,
                Note = transaction.Note
            };

            return View("Create", transactionVm);
        }

        [HttpPost]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        #region private methods
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

        
        public async Task<JsonResult> GetCategoriesJson([FromBody] OperationType transaction)
        {
            var userId = userService.GetUser();
            var cats = await GetCategories(userId, transaction);
            return Json(cats);
        }
        public async Task<JsonResult> GetCountsJson([FromBody] OperationType transaction)
        {
            bool condition = true;
            var userId = userService.GetUser();
            if(transaction.ToString() != "Ingreso")
            {
                condition = false;
            }
             var Count = await GetCounts(userId, condition);

            return Json(Count);

        }


        #endregion
    }
}

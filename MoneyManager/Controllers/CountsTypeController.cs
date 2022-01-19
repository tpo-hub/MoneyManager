using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class CountsTypeController : Controller
    {
        private readonly ICountTypeRepository countTypeRepository;
        private readonly IUserService userService;
        private readonly ITransactionRepository transactionRepository;

        public CountsTypeController(ICountTypeRepository countTypeRepository,
            IUserService userService, ITransactionRepository transactionRepository)
        {
            this.countTypeRepository = countTypeRepository;
            this.userService = userService;
            this.transactionRepository = transactionRepository;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        } 
        [HttpPost]
        public async Task<IActionResult> Create(CountType countType)
        {
            if(!ModelState.IsValid)
            {
                return View(countType);
            }
            countType.UserId = userService.GetUser();
            try
            {
              await countTypeRepository.Create(countType);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(nameof(countType.Name), ex.Message);
                return View(countType);
            }

            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public async Task<IActionResult> VerifyExistCountType(string name)
        //{
        //    if (await countTypeRepository.ExistCountType(null, name))
        //    {
        //        return Json($"El Nombre: {name} ya existe!");
        //    }

        //    return Json(true);
        //}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUser();
            var countsType = await countTypeRepository.Get(userId);
            return View(countsType);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userService.GetUser();
            var ct = await countTypeRepository.GetForId(id, userId);

            if (ct is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(ct);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CountType countType)
        {
            var userId = userService.GetUser();

            if (await countTypeRepository.ExistCountType(countType, null))
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await countTypeRepository.Update(countType);
           return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userService.GetUser();
            var ct = await countTypeRepository.GetForId(id, userId);

            if (ct is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            return View(ct);
        }   
        
        [HttpPost]
        public async Task<IActionResult> Delete(CountType countType)
        {
            var userId = userService.GetUser();
            var ct = await countTypeRepository.GetForId(countType.Id, userId);
            if (ct is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            await countTypeRepository.Delete(countType.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Sortable([FromBody] int[] ids)
        {
            var userId = userService.GetUser();
            var ct = await countTypeRepository.Get(userId);
            var idsCountTypes = ct.Select(x => x.Id);
            var countsTypesVerification = ids.Except(idsCountTypes).ToList();

            if (countsTypesVerification.Count > 0)
            {
                return Forbid();
            }

            List<CountType> orderedCounts = new List<CountType>();

            for(var i = 0; i < ids.Length; i++)
            {
                orderedCounts.Add(
                new CountType()
                {
                    Id = ids[i],
                    OrderType = i + 1 
                });
            }
            
            await countTypeRepository.Order(orderedCounts.AsEnumerable());

            return Ok();
        }

    }
}

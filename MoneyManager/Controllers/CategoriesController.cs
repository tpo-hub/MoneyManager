using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IRepositoryCategory repositoryCategory;
        private readonly IUserService userService;

        public CategoriesController(IRepositoryCategory repositoryCategory, IUserService userService)
        {
            this.repositoryCategory = repositoryCategory;
            this.userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = userService.GetUser();
            var model = await repositoryCategory.Get(userId);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = userService.GetUser();
            category.UserId = userId;
            await repositoryCategory.Create(category);
            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> Edit(int id)
        {
            var userId = userService.GetUser();
            var model = await repositoryCategory.GetForId(id, userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            var userId = userService.GetUser();
            category.UserId = userId;
            await repositoryCategory.Update(category);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = userService.GetUser();
            var model = await repositoryCategory.GetForId(id, userId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Category category)
        {
            await repositoryCategory.Delete(category.Id);
            return RedirectToAction("Index");
        }
    }
}

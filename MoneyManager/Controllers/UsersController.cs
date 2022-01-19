using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Models;
using MoneyManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            else
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Index", "Transaction");

            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
    }
}

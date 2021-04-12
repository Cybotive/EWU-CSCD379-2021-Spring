using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        static List<UserViewModel> Users = new List<UserViewModel>()
        {
            new UserViewModel { FirstName = "Bob", LastName = "Smith" },
            new UserViewModel { FirstName = "Sandra", LastName = "Miles" },
        };

        public IActionResult Index()
        {
            return View(Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel viewModel)
        {
            Users.Add(viewModel);
            return RedirectToAction(nameof(Index));
        }
    }
}

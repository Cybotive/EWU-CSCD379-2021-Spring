using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        static List<UserViewModel> _Users = new List<UserViewModel>()
        {
            new UserViewModel { FirstName = "Bob", LastName = "Smith" },
            new UserViewModel { FirstName = "Sandra", LastName = "Miles" },
        };

        public IActionResult Index()
        {
            return View(_Users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Users.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
         
        public IActionResult Edit(int id)
        {
            _Users[id].Id = id;
            return View(_Users[id]);
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Users[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _Users.RemoveAt(id);    
            return RedirectToAction(nameof(Index));
        }
    }
}

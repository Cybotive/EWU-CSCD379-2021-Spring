using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class GroupsController : Controller
    {
        static List<GroupViewModel> _Groups = new List<GroupViewModel>()
        {
            new GroupViewModel { GroupName = "Best Group" },
            new GroupViewModel { GroupName = "Mediocre People" },
        };

        public IActionResult Index()
        {
            return View(_Groups);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GroupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Groups.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        
        public IActionResult Edit(int id)
        {
            _Groups[id].Id = id;
            return View(_Groups[id]);
        }

        [HttpPost]
        public IActionResult Edit(GroupViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Groups[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _Groups.RemoveAt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

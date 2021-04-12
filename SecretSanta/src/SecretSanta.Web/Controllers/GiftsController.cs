using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class GiftsController : Controller
    {
        static List<GiftViewModel> _Gifts = new List<GiftViewModel>()
        {
            new GiftViewModel { Title = "Banana Costume", Desc = "A full body costume that transforms the wearer into a banana person.", Priority = 1 },
        };

        public IActionResult Index()
        {
            return View(_Gifts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GiftViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Gifts.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            _Gifts[id].Id = id;
            return View(_Gifts[id]);
        }

        [HttpPost]
        public IActionResult Edit(GiftViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _Gifts[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _Gifts.RemoveAt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Data;
using SecretSanta.Web.ViewModels;
using SecretSanta.Web.Api;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        public IUsersClient Client { get; }

        public UsersController(IUsersClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IActionResult> Index()
        {
            ICollection<FullUser> users = await Client.GetAllAsync();
            
            List<UserViewModel> viewModelUsers = new();
            foreach(var u in users)
            {
                viewModelUsers.Add(new UserViewModel{
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                });
            }

            return View(viewModelUsers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                await Client.PostAsync(new FullUser {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                });
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            FullUser userToConvert = await Client.GetAsync(id);

            if(userToConvert is null)
                return NotFound();

            UserViewModel userModel = new()
            {
                Id = userToConvert.Id,
                FirstName = userToConvert.FirstName,
                LastName = userToConvert.LastName
            };

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                int id = viewModel.Id ?? default(int);
                await Client.PutAsync(id, new UpdateUser {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                });
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await Client.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
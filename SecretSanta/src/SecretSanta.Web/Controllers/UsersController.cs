using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SecretSanta.Web.Api;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        public IUsersClient UserClient { get; }

        public UsersController(IUsersClient userClient)
        {
            UserClient = userClient ?? throw new ArgumentNullException(nameof(userClient));
        }

        public IActionResult Index() { return View(); }

        public IActionResult Create() { return View(); }

        public IActionResult Edit(int id) { return View(); }
    }
}

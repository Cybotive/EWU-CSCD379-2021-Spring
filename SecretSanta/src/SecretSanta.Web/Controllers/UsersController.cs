using System;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

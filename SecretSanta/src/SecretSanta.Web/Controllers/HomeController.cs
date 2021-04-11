using System;
using Microsoft.AspNetCore.Mvc;

namespace SecretSanta.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}

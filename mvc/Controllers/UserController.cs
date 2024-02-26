using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using mvc.Repositories;

namespace mvc.Controllers
{
    // [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepositories _userRepositories;

        public UserController(ILogger<UserController> logger, IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tblUser user)
        {

            int rowcount = _userRepositories.Login(user);
            if (rowcount == 1)
            {
                if (HttpContext.Session.GetString("role") == "admin")
                {
                    Console.WriteLine(HttpContext.Session.GetString("role"));
                    return RedirectToAction("Index", "Task");
                }
                else
                {
                    Console.WriteLine(HttpContext.Session.GetString("role"));
                    return RedirectToAction("TaskManager", "Task");
                }

            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(tblUser user)
        {
            user.c_role = "User";
            _userRepositories.Register(user);
            return RedirectToAction("Login");
        }
        



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
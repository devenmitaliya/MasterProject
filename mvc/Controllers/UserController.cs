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
                // var role  = HttpContext.Session.GetString("role");              
                return RedirectToAction("Index", "Employee");
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
            Console.WriteLine(user.c_role);
            _userRepositories.Register(user);
            Console.WriteLine(user.c_role);
            Console.WriteLine(user.c_uemail);
            Console.WriteLine(user.c_uid);
            Console.WriteLine(user.c_password);
            return RedirectToAction("Login");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
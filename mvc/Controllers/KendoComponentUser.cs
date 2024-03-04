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
    public class KendoComponentUser : Controller
    {
        private readonly ILogger<KendoComponentUser> _logger;
        private readonly IUserRepositories _userRepositories;

        public KendoComponentUser(ILogger<KendoComponentUser> logger,IUserRepositories userRepositories)
        {
            _logger = logger;
            _userRepositories=userRepositories;
        }

        public IActionResult Index()
        {
            return View();
        }

       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(tblUser user)
        {
           
                _userRepositories.Register(user);
                return RedirectToAction("Login");    
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(tblUser user)
        {
            if (_userRepositories.Login(user)==1)
            {
                var role = HttpContext.Session.GetString("role");
                if(role == "Admin"){

                return RedirectToAction("Admin","KendoComponentEmployee");
                }else{
                return RedirectToAction("Index","KendoComponentEmployee");

                }
            }
            else
            {
                return Ok("wrong");
            }

        }


 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
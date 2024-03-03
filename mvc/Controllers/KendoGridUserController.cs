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
    public class KendoGridUserController : Controller
    {
        private readonly ILogger<KendoGridUserController> _logger;
                private readonly IUserRepositories _userRepo;


        public KendoGridUserController(ILogger<KendoGridUserController> logger, IUserRepositories userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        // public IActionResult RegisterUser([FromBody] tblUser user)
        // {
        //     try
        //     {
        //         _userRepo.Register(user);

        //         return Json(new { success = true });
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { success = false, error = ex.Message });
        //     }
        // }

        // [HttpPost]
        // public IActionResult Login([FromBody] tblUser user)
        // {
        //     try
        //     {
        //         int rowCount = _userRepo.Login(user);

        //         if (rowCount > 0)
        //         {
        //             var role = HttpContext.Session.GetString("role");
        //             var username = HttpContext.Session.GetString("username");

        //             return Json(new { success = true, role = role, username = username });

        //         }
        //         else
        //         {
        //             // Login failed
        //             return Json(new { success = false, error = "Invalid credentials" });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         return Json(new { success = false, error = ex.Message });
        //     }
        // }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
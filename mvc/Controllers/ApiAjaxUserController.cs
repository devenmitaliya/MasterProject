using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mvc.Controllers
{
    // [Route("[controller]")]
    public class ApiAjaxUserController : Controller
    {
        private readonly ILogger<ApiAjaxUserController> _logger;

        public ApiAjaxUserController(ILogger<ApiAjaxUserController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

//  [Route("Login")]
        public IActionResult Login(string username)
        {
             ViewBag.username = username;
            return View();
        }

        //  [Route("Register")]
        public IActionResult Register() 
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
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
    public class ApiKendoComponentEmployeeController : Controller
    {
        private readonly ILogger<ApiKendoComponentEmployeeController> _logger;

        public ApiKendoComponentEmployeeController(ILogger<ApiKendoComponentEmployeeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string username)
        {
            ViewData["Username"] = username;

            return View();
        }
        public IActionResult Dashboard(string username)
        {
            ViewData["Username"] = username;
            // var username1 = ViewBag.username; // Assuming ViewBag.username contains the dynamic username
            HttpContext.Session.SetString("username", username);
            var ok = HttpContext.Session.GetString("username");

            //  ViewBag.role = role;
            Console.WriteLine("IDDDDD::::::::::::::: " + ViewBag.username);
            Console.WriteLine("OKOKOKOKOKOKOKOKOKO::::::::::::::: " + ok);
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.id = id;
            Console.WriteLine("IDDDDD::::::::::::::: " + ViewBag.id);
            return View();
        }
        [HttpGet]
        public IActionResult Add()
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
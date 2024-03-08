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
    public class ApiAjaxEmployeeController : Controller
    {
        private readonly ILogger<ApiAjaxEmployeeController> _logger;

        public ApiAjaxEmployeeController(ILogger<ApiAjaxEmployeeController> logger)
        {
            _logger = logger;
        }


        // [HttpGet]
        public IActionResult Index(string username)
        {
            ViewData["Username"] = username;
            // HttpContext.Session.SetString("username", username);
            // var ok = HttpContext.Session.GetString("username");
            // Console.WriteLine("ADMIN ADMIN::::::::::::::: " + ok);


            //  ViewBag.role = role;

            return View();
        }
        public IActionResult Dashboard(string username)
        {
            ViewData["Username"] = username;
            // var username1 = ViewBag.username; // Assuming ViewBag.username contains the dynamic username
            HttpContext.Session.SetString("username", username);
            var uname = HttpContext.Session.GetString("username");

            //  ViewBag.role = role;
            Console.WriteLine("IDDDDD::::::::::::::: " + ViewBag.username);
            // Console.WriteLine("OKOKOKOKOKOKOKOKOKO::::::::::::::: " + ok);


            return View();
        }
        // [Route("Update")]
        // [HttpGet("{id}")]





        public IActionResult Update(int id)
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
        [HttpGet]
        public IActionResult Details()
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
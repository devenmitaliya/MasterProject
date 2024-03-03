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
    public class ApiKendoComponentEmployeeController : Controller
    {
        private readonly ILogger<ApiKendoComponentEmployeeController> _logger;
        // private readonly IEmployeeRepository _employeeRepository;

        public ApiKendoComponentEmployeeController(ILogger<ApiKendoComponentEmployeeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index( )
        {
            return View();
        }

         public IActionResult Update(int id)
        {
            ViewBag.id = id;
            Console.WriteLine("IDDDDD::::::::::::::: "+ViewBag.id);
            return View();
        }
         [HttpGet]
        public IActionResult Add(){
            
             return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
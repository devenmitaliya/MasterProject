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
        public IActionResult Index()
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
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
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        // private readonly IHttpContextAccessor _httpContextAccessor;



        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            // _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Add(tblEmployee emp)
        {
            _employeeRepository.AddEmployee(emp);
            return View("Index", "Employee");
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            // if (HttpContext.Session.GetString("role") == "admin")
            // {
            var employee = _employeeRepository.GetOneEmployee(id);

            return View(employee);
            // }
            // return RedirectToAction("Login", "User");
        }


        [HttpPost]
        public IActionResult Edit(int id, tblEmployee employee)
        {

            // Assuming 'admin' username is checked here
            // if (HttpContext.Session.GetString("role") == "admin")
            // {

            _employeeRepository.EditEmployee(employee);
            return RedirectToAction("Index", "Employee");

            // }
            // return RedirectToAction("Login", "User");
        }



        public IActionResult Delete(int id)
        {
            // Assuming 'admin' username is checked here
            // if (HttpContext.Session.GetString("role") == "admin")
            // {
            var employee = _employeeRepository.GetOneEmployee(id);
            _employeeRepository.DeleteEmployee(employee);

            return View("Index", "Employee");
            // }
            // return RedirectToAction("Login", "User");
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
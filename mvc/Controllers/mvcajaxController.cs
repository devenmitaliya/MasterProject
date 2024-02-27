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
    //[Route("[controller]")]
    public class mvcajaxController : Controller
    {
        private readonly ILogger<mvcajaxController> _logger;

        private readonly IEmployeeRepository _employeeRepository;


        public mvcajaxController(ILogger<mvcajaxController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _employeeRepository.GetAllEmployee();
            return Json(employees);
        }

        [HttpPost]
        public IActionResult Add(tblEmployee emp)
        {
            _employeeRepository.AddEmployee(emp);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Edit(int id, tblEmployee employee)
        {
            _employeeRepository.EditEmployee(employee);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.GetOneEmployee(id);
            _employeeRepository.DeleteEmployee(employee);
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = _employeeRepository.GetOneEmployee(id);
            if (employee == null)
            {
                return NotFound(); 
            }
            return Json(employee);
        }

        [HttpGet]
        public IActionResult Department()
        {
            return Json(_employeeRepository.GetAllDepartment());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
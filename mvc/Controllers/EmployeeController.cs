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
            // if (emp.c_empshift != null)
            // {
            //     string selectedShifts = string.Join(",", emp.c_empshift);
            //     emp.c_empshift = selectedShifts; // Assign the concatenated string back to the property
            // }
            // Console.WriteLine(emp.c_empshift);
            _employeeRepository.AddEmployee(emp);
            return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                var employee = _employeeRepository.GetOneEmployee(id);

                return View(employee);
            }
            return RedirectToAction("Index", "Employee");
        }


        [HttpPost]
        public IActionResult Edit(int id, tblEmployee employee)
        {

            if (HttpContext.Session.GetString("role") == "Admin")
            {

                _employeeRepository.EditEmployee(employee);
                return RedirectToAction("Index", "Employee");

            }
            return RedirectToAction("Index", "Employee");
        }



        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                var employee = _employeeRepository.GetOneEmployee(id);
                _employeeRepository.DeleteEmployee(employee);

                return RedirectToAction("Index", "Employee");
            }
            return RedirectToAction("Index", "Employee");
        }

        public IActionResult Department()
        {
            return Json(_employeeRepository.GetAllDepartment());
        }
        public IActionResult Details(int id)
        {
            tblEmployee emp = _employeeRepository.GetOneEmployee(id);

            return View(emp);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
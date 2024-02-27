using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mvc.Models;
using mvc.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mvc.Controllers
{
    [Route("[controller]")]
    public class KendoGridController : Controller
    {
        private readonly ILogger<KendoGridController> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        public KendoGridController(ILogger<KendoGridController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Produces("application/json")]
        public IActionResult GetAllEmployee()
        {
            List<tblEmployee> emp = _employeeRepository.GetAllEmployee();
            return Json(emp);
        }

        public IActionResult Details(int id)
        {
            tblEmployee emp = _employeeRepository.GetOneEmployee(id);
            if (emp == null)
            {
                return NotFound();
            }
            return Json(emp);
        }

        public IActionResult Create(tblEmployee emp)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.AddEmployee(emp);
                return Json(new { success = true, message = "ok" });
            }
            return Json(new { success = false, message = "ok" });
        }

         public IActionResult Edit(tblEmployee emp)
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.EditEmployee(emp);
                return Json(new { success = true, message = "ok" });
            }
            return Json(new { success = false, message = "not ok" });
        }

        public IActionResult Delete(tblEmployee emp)
        {
            try
            {
               
                _employeeRepository.DeleteEmployee(emp);
                return Json(new { success = true, message = "ok" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "not ok" });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
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
    // [Route("[controller]")]
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

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAllEmployee()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
            return Json(employees);
        }
        
        [HttpGet]
        public IActionResult Viewdept(){
            List<tblDepartment> departments = _employeeRepository.GetAllDepartment();
            return Json(departments);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            tblEmployee employee = _employeeRepository.GetOneEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Json(employee);
        }

        [HttpPost]
        public IActionResult Add(tblEmployee emp)
        {
            try
            {
                // Your logic to add the employee to the database
                _employeeRepository.AddEmployee(emp);
                return Json(new { success = true, message = "Employee added successfully", data = emp });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EditEmployee(tblEmployee emp)
        {
            try
            {
                _logger.LogInformation($"Received emp: {emp}");
                _logger.LogInformation("Employee edited successfully");

                // Log or debug statements to inspect emp and ensure it has the correct values
                _employeeRepository.EditEmployee(emp);
                return Json(new { success = true, message = "Employee edited successfully", data = emp });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }



        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                var employee = _employeeRepository.GetOneEmployee(id);
                if (employee == null)
                {
                    return Json(new { success = false, message = "Employee not found" });
                }

                _employeeRepository.DeleteEmployee(employee);
                return Json(new { success = true, message = "Employee deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }

}
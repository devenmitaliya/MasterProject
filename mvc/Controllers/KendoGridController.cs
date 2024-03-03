using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using mvc.Models;
using mvc.Repositories;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace mvc.Controllers
{
    // [Route("[controller]")]
    public class KendoGridController : Controller
    {
        private readonly ILogger<KendoGridController> _logger;
        //private string file; // declare it at the class level
        private readonly IEmployeeRepository _employeeRepository;
        // private readonly IWebHostEnvironment _hostingEnvironment;
        // private readonly IHostingEnvironment _hostingEnviroment;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public KendoGridController(ILogger<KendoGridController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;

            // _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }


        [HttpPost]

        public IActionResult UploadPhoto(tblEmployee emp, IFormFile photo)
        {
            if (photo != null)
            {
                string filename = photo.FileName;
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", filename);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {

                    photo.CopyTo(stream);
                }

                file = filename;

            }

            return Json("Image Uploaded");
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult GetAllEmployee()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
            return Json(employees);
        }

        [HttpGet]
        public IActionResult GetDataFromUser()
        {
            try
            {
                var username = HttpContext.Session.GetString("username");
                List<tblEmployee> employees = _employeeRepository.GetEmployeeFromUserName(username);
                return Json(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving data." });
            }
        }

        [HttpGet]
        public IActionResult Viewdept()
        {
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
        static string file = "";

        [HttpPost]
        public IActionResult Add(tblEmployee emp)
        {
            // try
            // {
            // string filename = Path.GetFileName(emp.c_empimg.FileName);
            // string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "images", filename);

            // using (var stream = new FileStream(filepath, FileMode.Create))
            // {
            //     emp.c_empimg.CopyTo(stream);
            // }

            // Console.WriteLine("Photo Add" + filename);
            // file = filename;
            emp.c_empimg = file;
            _employeeRepository.AddEmployee(emp);
            return Json(new { success = true, message = "Employee added successfully", data = emp });
            // }
            // catch (Exception ex)
            // {
            //     return Json(new { success = false, message = ex.Message });
            // }
        }


        [HttpPost]
        public IActionResult EditEmployee(tblEmployee emp)
        {
            // try
            // {

            //     _logger.LogInformation($"Received emp: {emp}");
            //     _logger.LogInformation("Employee edited successfully");

            // Log or debug statements to inspect emp and ensure it has the correct values
            emp.c_empimg = file;
            _employeeRepository.EditEmployee(emp);
            return Json(new { success = true, message = "Employee edited successfully", data = emp });
            // }
            // catch (Exception ex)
            // {
            //     return Json(new { success = false, message = ex.Message });
            // }
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
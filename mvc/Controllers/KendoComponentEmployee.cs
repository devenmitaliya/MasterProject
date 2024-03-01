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
    public class KendoComponentEmployee : Controller
    {
        private readonly ILogger<KendoComponentEmployee> _logger;

        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public KendoComponentEmployee(IWebHostEnvironment hostingEnvironment, ILogger<KendoComponentEmployee> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            var emp = _employeeRepository.GetAllEmployee();

            if (emp == null)
            {
                return View("Error");
            }

            return View(emp);
        }

        [Produces("application/json")]
        public IActionResult GetAllEmployee()
        {
            List<tblEmployee> emp = _employeeRepository.GetAllEmployee();
            return Json(emp);
        }

        public IActionResult GetAllDepartment()
        {
            List<tblDepartment> dept = _employeeRepository.GetAllDepartment();
            return Json(dept);
        }

       public IActionResult Details(int id)
            {
                var employee = _employeeRepository.GetOneEmployee(id);
                if (employee == null)
                {
                    return NotFound();
                }
                //  ViewBag.Title = "Employee Details";
                return View(employee);
            }



        //  static string file = "";

        [HttpGet]
        public IActionResult Add()
        {
            var departments = _employeeRepository.GetAllDepartment();
            ViewBag.Departments = departments;
            return View();
        }


        [HttpPost]
        public IActionResult Add(tblEmployee emp, IFormFile c_empimg)
        {
            try
            {
                if (c_empimg != null && c_empimg.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    string uniqueFilename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(c_empimg.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        c_empimg.CopyTo(stream);
                    }

                    emp.c_empimg = uniqueFilename;
                }

                _employeeRepository.AddEmployee(emp);

                // return Ok("Employee Added Successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding employee: " + ex.Message);
                return StatusCode(500, "An error occurred while adding the employee.");
            }
        }



    [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {

                tblEmployee employeeToDelete = _employeeRepository.GetOneEmployee(id);
            
                _employeeRepository.DeleteEmployee(employeeToDelete);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting employee: {ex.Message}");
                return StatusCode(500, "An error occurred while deleting the employee.");
            }
        }



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _employeeRepository.GetOneEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            var departments = _employeeRepository.GetAllDepartment();
            ViewBag.Departments = departments;

            return View(employee);
        }


       [HttpPost]
public IActionResult Edit(tblEmployee emp, IFormFile c_empimg)
{
    try
    {
        if (c_empimg != null && c_empimg.Length > 0)
        {
            var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            string uniqueFilename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(c_empimg.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFilename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                c_empimg.CopyTo(stream);
            }

            emp.c_empimg = uniqueFilename;
        }

        var department = Request.Form["c_empdepartment"].ToString();
        emp.c_empdepartment = department;

        _employeeRepository.EditEmployee(emp);
        return RedirectToAction("Index");
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine("Error Updating Employee: " + ex.Message);
        // Return an error response
        return StatusCode(500, "An error occurred while Updating the Employee.");
    }
}





        // public IActionResult UploadPhoto(tblEmployee emp)
        // {
        //     if (emp.c_empimg != null)
        //     {
        //         string filename = emp.c_empimg.FileName;
        //         string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", filename);

        //         using (var stream = new FileStream(filepath, FileMode.Create))
        //         {

        //             emp.c_empimg.CopyTo(stream);
        //         }

        //         file = filename;

        //     }

        //     return Json("Image Uploaded");
        // }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
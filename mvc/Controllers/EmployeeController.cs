using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc.Models;
using mvc.Repositories;
using mvc.Models; // Adjust namespace as necessary


namespace mvc.Controllers
{
    // [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;

        private readonly IWebHostEnvironment _hostingEnvironment;

        // private readonly IHttpContextAccessor _httpContextAccessor;



        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }
        static string file = "";


        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("role") == "Admin")
            {
                List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
                return View(employees);
            }
            else
            {
                var user = HttpContext.Session.GetString("username");
                Console.WriteLine("USER    : : : : : : ::::    " + user);
                List<tblEmployee> employees = _employeeRepository.GetEmployeeFromUserName(user);
                return View(employees);
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            // var user = HttpContext.Session.GetString("username");
            // Console.WriteLine("USER    : : :", user);
            return View();
        }

        public IActionResult Add(tblEmployee emp)
        {
            if (emp.photo != null && emp.photo.Length > 0)
            {
                try
                {
                    string filename = Path.GetFileName(emp.photo.FileName);
                    string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", filename);

                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        emp.photo.CopyTo(stream);
                    }

                    Console.WriteLine("Photo uploaded successfully: " + filename);
                    file = filename;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error uploading photo: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("No photo uploaded.");
            }
            emp.c_empimg = file;
            // var user = HttpContext.Session.GetString("username");
            // Console.WriteLine("USER    : : :", user);
            // emp.c_username = user;
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
        public IActionResult Edit(tblEmployee employee)
        {
            if (HttpContext.Session.GetString("role") == "Admin")
            {
                if (employee.photo != null && employee.photo.Length > 0)
                {
                    try
                    {
                        string filename = Path.GetFileName(employee.photo.FileName);
                        string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", filename);

                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            employee.photo.CopyTo(stream);
                        }

                        Console.WriteLine("Photo uploaded successfully: " + filename);
                        file = filename;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error uploading photo: " + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("No photo uploaded.");
                }
                employee.c_empimg = file;

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


        public IActionResult UploadPhoto(tblEmployee employee)
        {


            return Json("Image Uploaded");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
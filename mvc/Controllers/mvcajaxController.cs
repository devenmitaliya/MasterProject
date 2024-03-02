using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
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
        private readonly IWebHostEnvironment _hostingEnvironment;
        // private string file;


        public mvcajaxController(ILogger<mvcajaxController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Admin()
        {
            if(HttpContext.Session.GetString("username")!= "" && HttpContext.Session.GetString("role") == "Admin"){
            return View();
            }else{
                return RedirectToAction("Login", "ajaxuser");
            }
        }
        public IActionResult User()
        {
            if(HttpContext.Session.GetString("username")!= "" && HttpContext.Session.GetString("role") != ""){
            return View();
            }else{
                return RedirectToAction("Login", "ajaxuser");
            }
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _employeeRepository.GetAllEmployee();
            return Json(employees);
        }

        [HttpPost]
        public IActionResult Add(tblEmployee emp, IFormFile photo)
        {
            if (photo != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");


                string uniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                emp.c_empimg = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      " + emp.c_empimg);
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }


            Console.WriteLine(emp.c_empimg);
            _employeeRepository.AddEmployee(emp);
            return Json(new { success = true });
        }

        [HttpPut]
        public IActionResult Edit(int id, tblEmployee employee, IFormFile photo)
        {
            if (photo != null)
            {
                var uploadsFolder = Path.Combine("D:\\GitProject\\MasterProject\\mvc\\wwwroot", "uploads");
                string uniqueFilename = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                employee.c_empimg = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      " + employee.c_empimg);
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }

            employee.c_empid = id;
            _employeeRepository.EditEmployee(employee);
            return Json(new { success = true });
        }

        [HttpDelete]
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


        [HttpGet("/mvcajax/GetDataFromUsername/{name}")]
        public IActionResult GetDataFromUsername(string name)
        {
            return Json(_employeeRepository.GetEmployeeFromUserName(name));
        }

        [HttpPost]
        public IActionResult SetSessionValue(string key, string value)
        {
            // Set the session value
            HttpContext.Session.SetString("role","");
            HttpContext.Session.SetString("username","");

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
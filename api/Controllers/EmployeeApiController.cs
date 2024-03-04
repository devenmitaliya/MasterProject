using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Repositories;

// api controllers

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeApiController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public EmployeeApiController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _employeeRepository = employeeRepository;
            _hostEnvironment = hostEnvironment;
            _httpContextAccessor = httpContextAccessor;

        }




        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
            {
                return RedirectToAction("Login");
            }

            return Ok();
        }


        [HttpGet("Index")]
        public IActionResult Index()
        {
            var session = HttpContext.Session;
            if (string.IsNullOrEmpty(session.GetString("username")) || string.IsNullOrEmpty(session.GetString("email")))
            {
                return RedirectToAction("Login");
            }

            return Ok();
        }





        [HttpGet]
        public IActionResult GetAll()
        {
            List<tblEmployee> employees = _employeeRepository.GetAllEmployee();
            return Ok(employees);
        }


        [HttpGet("api/Department")]
        public IActionResult Department()
        {
            List<tblDepartment> departments = _employeeRepository.GetAllDepartment();
            return Ok(departments);
        }
        [HttpPost]
        public IActionResult Add([FromForm] tblEmployee emp)
        {

            if (emp.photo != null)
            {
                //  var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "images");

                // var uploadsFolder = Path.Combine("/Users/yashveekotadiya/Desktop/newmaster/MasterProject/mvc/wwwroot/", "images");
                var uploadsFolder = Path.Combine("G:\\MasterProject\\mvc\\wwwroot", "uploads");
                string uniqueFilename = Guid.NewGuid().ToString() + "_" + emp.photo.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    emp.photo.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                emp.c_empimg = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      " + emp.c_empimg);
            }
            else
            {
                Console.WriteLine("NOT FOUND");
            }

            Console.WriteLine(emp);
            _employeeRepository.AddEmployeeApi(emp);
            return Ok("Employee added successfully");
        }








        [HttpGet("current/{username}")]
        public IActionResult GetEmployeesForCurrentUser(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest("Username not provided");
                }

                // Get role from session or any other source
                string role = _httpContextAccessor.HttpContext.Session.GetString("role");

                // Check if the user is an admin
                if (!string.IsNullOrEmpty(role) && role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    // If user is admin, return all employees
                    List<tblEmployee> allEmployees = _employeeRepository.GetAllEmployee();
                    return Ok(allEmployees);
                }
                else
                {
                    // If user is not admin, return employees for the current user
                    List<tblEmployee> employees = _employeeRepository.GetEmployeeFromUserName(username);
                    return Ok(employees);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("admin")]
        public IActionResult GetEmployeesForAdmin()
        {
            try
            {
                // Get role from session
                string role = _httpContextAccessor.HttpContext.Session.GetString("role");

                // Check if the user is an admin
                if (!string.IsNullOrEmpty(role) && role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    // If user is admin, return all employees
                    List<tblEmployee> allEmployees = _employeeRepository.GetAllEmployee();
                    return Ok(allEmployees);
                }
                else
                {
                    // If user is not admin, return Forbidden status
                    return StatusCode(403, "Forbidden");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }





        [HttpPut]
        public IActionResult Edit1([FromForm] tblUpdate emp)
        {
            _employeeRepository.EditEmployeeApi(emp);
            return Ok("Data update successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var employee = _employeeRepository.GetOneEmployee(id);
            _employeeRepository.DeleteEmployee(employee);
            return Ok("Employee deleted successfully");
        }

        [HttpGet("{id}")]
        public IActionResult GetOneEmployee(int id)
        {
            var employee = _employeeRepository.GetOneEmployee(id);
            return Ok(employee);
        }
    }
}
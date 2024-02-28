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
        private string file; // declare it at the class level
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        // private readonly IHostingEnvironment _hostingEnviroment;

        public KendoGridController(ILogger<KendoGridController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
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

        // [HttpGet]
        // public IActionResult Add()
        // {
        //     // Assuming you have a method to retrieve departments, replace it with your actual logic
        //     var departments = _employeeRepository.GetAllDepartment();
        //     ViewBag.Departments = departments; // Pass departments to ViewBag
        //     return View();
        // }


        [HttpPost]
public async Task<IActionResult> Add(tblEmployee emp, IFormFile temp_name)
{
    try
    {
        Console.WriteLine("Add action called.");
        if (temp_name != null && temp_name.Length > 0)
        {
            var uploadsFolder = Path.Combine(@"D:\\casepoint Internship\\MasterProject\\mvc\\wwwroot", "images");

            string uniqueFilename = Guid.NewGuid().ToString() + "_" + Path.GetFileName(temp_name.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFilename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await temp_name.CopyToAsync(stream);
            }

            emp.c_empimg = uniqueFilename;
        }
        else
        {
            Console.WriteLine("Image not found");
        }

        _employeeRepository.AddEmployee(emp);

        return Ok("Employee Added Successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error adding employee: " + ex.Message);
        return StatusCode(500, "An error occurred while adding the employee.");
    }
}


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }

}
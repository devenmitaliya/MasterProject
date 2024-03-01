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
        private readonly IWebHostEnvironment hostEnvironment;


        public EmployeeApiController(IEmployeeRepository employeeRepository, IWebHostEnvironment hostEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostEnvironment = hostEnvironment;
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
        public IActionResult Add([FromForm] tblEmployee emp, IFormFile ephoto)
        {

            if (ephoto != null)
            {
                var uploadsFolder = Path.Combine("/Users/yashveekotadiya/Desktop/new master/MasterProject/mvc/wwwroot/", "images");
                string uniqueFilename = Guid.NewGuid().ToString() + "_" + ephoto.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    ephoto.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                emp.c_empimg = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      "+emp.c_empimg);
            }else{
                Console.WriteLine("NOT FOUND");
            }




            Console.WriteLine(emp);
            _employeeRepository.AddEmployee(emp);
            return Ok("Employee added successfully");
        }

        [HttpPut]
        public IActionResult Edit([FromForm]tblEmployee emp, IFormFile ephoto)
        {
if (ephoto != null)
            {
                var uploadsFolder = Path.Combine("/Users/yashveekotadiya/Desktop/New/MVC/wwwroot/", "images");
                string uniqueFilename = Guid.NewGuid().ToString() + "_" + ephoto.FileName;
                string filepath = Path.Combine(uploadsFolder, uniqueFilename);


                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    ephoto.CopyTo(stream);
                }

                Console.WriteLine("Upload PHOTO ::::    " + uniqueFilename);
                emp.c_empimg = uniqueFilename;

                Console.WriteLine("C IMAGE : : : : :      "+emp.c_empimg);
            }else{
                Console.WriteLine("NOT FOUND");
            }









            _employeeRepository.EditEmployee(emp);
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
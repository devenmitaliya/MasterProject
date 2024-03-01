using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace mvc.Repositories
{
    public interface IEmployeeRepository
    {
        List<tblEmployee> GetAllEmployee();

        tblEmployee GetOneEmployee(int id);

        void AddEmployee(tblEmployee emp);

        void EditEmployee(tblEmployee emp);

        void DeleteEmployee(tblEmployee emp);

        List<tblDepartment> GetAllDepartment();

        List<tblEmployee> GetEmployeeFromUserName(string user);
    }
}
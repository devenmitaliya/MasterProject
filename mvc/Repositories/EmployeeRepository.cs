using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;

namespace mvc.Repositories
{
    public class EmployeeRepository:CommanRepository,IEmployeeRepository
    {
        public List<tblEmployee> GetAllEmployee(){
            List<tblEmployee> empList = new List<tblEmployee>();

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment FROM t_employee e, t_department d WHERE e.c_empdepartment = d.c_departmentid";

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var emp = new tblEmployee
                        {
                            c_empid = Convert.ToInt32(dr["c_empid"]),
                            c_empname = dr["c_empname"].ToString(),
                            c_empgender = dr["c_empgender"].ToString(),                            
                            c_empdob = DateTime.Parse(dr["c_empdob"].ToString()),
                            c_empshift = dr["c_empshift"].ToString(),
                            c_empimg = dr["c_empimg"].ToString(),
                            c_empdepartment = Convert.ToInt32(dr["c_empdepartment"]),
                        };
                        empList.Add(emp);

                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return empList;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;

namespace mvc.Repositories
{
    public class EmployeeRepository : CommanRepository, IEmployeeRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<tblEmployee> GetAllEmployee()
        {
            List<tblEmployee> empList = new List<tblEmployee>();

            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment, c_departmentname FROM t_employee e, t_department d WHERE e.c_empdepartment = d.c_departmentid ORDER BY c_empid";

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
                            // c_empshift = !string.IsNullOrEmpty(dr["c_empshift"].ToString()) ? dr["c_empshift"].ToString().Split(",").ToList() : new List<string>(),

                            c_empshift = dr["c_empshift"].ToString().Split(',').ToList(),
                            // c_empshift = dr["c_empshift"].ToString(),
                            c_empimg = dr["c_empimg"].ToString(),
                            c_empdepartment = dr["c_empdepartment"].ToString(),
                            department = dr["c_departmentname"].ToString()
                            // c_empdepartment = Convert.ToInt32(dr["c_empdepartment"]),
                        };
                        empList.Add(emp);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return empList;
        }


        public tblEmployee GetOneEmployee(int id)
        {
            var emp = new tblEmployee();

            var cmd = new NpgsqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment FROM t_employee e, t_department d WHERE e.c_empdepartment = d.c_departmentid AND c_empid = @empid";
                cmd.Parameters.AddWithValue("@empid", id);
                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        emp.c_empid = Convert.ToInt32(dr["c_empid"]);
                        emp.c_empname = dr["c_empname"].ToString();
                        emp.c_empgender = dr["c_empgender"].ToString();
                        emp.c_empdob = DateTime.Parse(dr["c_empdob"].ToString());
                        // emp.c_empshift = dr["c_empshift"].ToString().Split(",").ToList();
                        emp.c_empshift = dr["c_empshift"].ToString().Split(",").ToList();
                        emp.c_empimg = dr["c_empimg"].ToString();
                        emp.c_empdepartment = dr["c_empdepartment"].ToString();
                        // emp.c_empdepartment = Convert.ToInt32(dr["c_empdepartment"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return emp;
        }


        public void AddEmployee(tblEmployee emp)
        {
            try
            {
                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                int deptId = GetDepartmentId(emp.c_empdepartment, conn);
                Console.WriteLine("Department ID ::::::" + deptId);
                string shifts = string.Join(",", emp.c_empshift);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO t_employee(c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment, c_username) VALUES ( @c_empname, @c_empgender, @c_empdob, @c_empshift, @c_empimg, @c_empdepartment, @c_username)";

                var session = _httpContextAccessor.HttpContext.Session;
                var user = session.GetString("username");
                Console.WriteLine("Final ADDED User " + user);

                cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
                cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
                cmd.Parameters.AddWithValue("@c_empdob", emp.c_empdob);
                cmd.Parameters.AddWithValue("@c_empshift", shifts);
                cmd.Parameters.AddWithValue("@c_empimg", emp.c_empimg);
                cmd.Parameters.AddWithValue("@c_empdepartment", deptId);
                cmd.Parameters.AddWithValue("@c_username", user);

                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }



        private int GetDepartmentId(string c_departmentname, NpgsqlConnection conn)
        {
            int deptId = 0;
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT c_departmentid FROM t_department WHERE c_departmentname = @departmentname", conn))
                {
                    cmd.Parameters.AddWithValue("@departmentname", c_departmentname);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            deptId = reader.GetInt32(0);
                        }
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return deptId;
        }

        public void EditEmployee(tblEmployee emp)
        {
            try
            {
                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                Console.WriteLine(emp.c_empdepartment);
                int deptId = GetDepartmentId(emp.c_empdepartment, conn);
                cmd.CommandType = CommandType.Text;
                string shifts = string.Join(",", emp.c_empshift);


                // cmd.CommandText = "UPDATE t_employee SET c_empname=@c_empname , c_empgender=@c_empgender , c_empdob=@c_empdob , c_empshift=@c_empshift , c_empimg=@c_empimg , c_empdepartment=@c_empdepartment WHERE c_empid =@c_empid ";
                cmd.CommandText = "UPDATE t_employee SET  c_empshift=@c_empshift , c_empdepartment=@c_empdepartment WHERE c_empid =@c_empid ";

                cmd.Parameters.AddWithValue("@c_empid", emp.c_empid);
                // cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
                // cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
                // cmd.Parameters.AddWithValue("@c_empdob", emp.c_empdob);
                cmd.Parameters.AddWithValue("@c_empshift", shifts);
                // cmd.Parameters.AddWithValue("@c_empimg", emp.c_empimg);
                cmd.Parameters.AddWithValue("@c_empdepartment", deptId);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }








        public void DeleteEmployee(tblEmployee emp)
        {
            try
            {

                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM t_employee WHERE c_empid = @c_empid";

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@c_empid", emp.c_empid);
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }



        public List<tblDepartment> GetAllDepartment()
        {
            List<tblDepartment> departments = new List<tblDepartment>();
            var cmd = new NpgsqlCommand();

            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c_departmentid,c_departmentname FROM t_department";

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var department = new tblDepartment
                        {
                            c_departmentid = Convert.ToInt32(dr["c_departmentid"]),
                            c_departmentname = dr["c_departmentname"].ToString()
                        };
                        departments.Add(department);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }

            return departments;
        }


        public List<tblEmployee> GetEmployeeFromUserName(string user)
        {
            Console.WriteLine("USER DATA :: : : : : " + user);
            List<tblEmployee> empList = new List<tblEmployee>();
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT c_empid, c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment, c_departmentname FROM t_employee e, t_department d WHERE e.c_empdepartment = d.c_departmentid AND c_username = @username  ORDER BY c_empid ";

                cmd.Parameters.AddWithValue("@username", user);

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var employee = new tblEmployee
                        {
                            c_empid = Convert.ToInt32(dr["c_empid"]),
                            c_empname = dr["c_empname"].ToString(),
                            c_empgender = dr["c_empgender"].ToString(),
                            c_empdob = DateTime.Parse(dr["c_empdob"].ToString()),
                            // c_empshift = !string.IsNullOrEmpty(dr["c_empshift"].ToString()) ? dr["c_empshift"].ToString().Split(",").ToList() : new List<string>(),

                            c_empshift = dr["c_empshift"].ToString().Split(',').ToList(),
                            // c_empshift = dr["c_empshift"].ToString(),
                            c_empimg = dr["c_empimg"].ToString(),
                            c_empdepartment = dr["c_empdepartment"].ToString(),
                            department = dr["c_departmentname"].ToString()
                            // c_empdepartment = Convert.ToInt32(dr["c_empdepartment"]),
                        };
                        empList.Add(employee);

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return empList;
        }

        public void EditEmployeeApi(tblUpdate emp)
        {
            try
            {
                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                Console.WriteLine(emp.c_empdepartment);
                int deptId = GetDepartmentId(emp.c_empdepartment, conn);
                cmd.CommandType = CommandType.Text;
                string shifts = string.Join(",", emp.c_empshift);


                // cmd.CommandText = "UPDATE t_employee SET c_empname=@c_empname , c_empgender=@c_empgender , c_empdob=@c_empdob , c_empshift=@c_empshift , c_empimg=@c_empimg , c_empdepartment=@c_empdepartment WHERE c_empid =@c_empid ";
                cmd.CommandText = "UPDATE t_employee SET  c_empshift=@c_empshift , c_empdepartment=@c_empdepartment WHERE c_empid =@c_empid ";

                cmd.Parameters.AddWithValue("@c_empid", emp.c_empid);
                // cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
                // cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
                // cmd.Parameters.AddWithValue("@c_empdob", emp.c_empdob);
                cmd.Parameters.AddWithValue("@c_empshift", shifts);
                // cmd.Parameters.AddWithValue("@c_empimg", emp.c_empimg);
                cmd.Parameters.AddWithValue("@c_empdepartment", deptId);

                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public void AddEmployeeApi(tblEmployee emp)
        {
            try
            {
                conn.Open();
                var cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                int deptId = GetDepartmentId(emp.c_empdepartment, conn);
                Console.WriteLine("Department ID ::::::" + deptId);
                string shifts = string.Join(",", emp.c_empshift);
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO t_employee(c_empname, c_empgender, c_empdob, c_empshift, c_empimg, c_empdepartment, c_username) VALUES ( @c_empname, @c_empgender, @c_empdob, @c_empshift, @c_empimg, @c_empdepartment, @name)";

                // var session = _httpContextAccessor.HttpContext.Session;
                // var user = session.GetString("username");

                cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
                cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
                cmd.Parameters.AddWithValue("@c_empdob", emp.c_empdob);
                cmd.Parameters.AddWithValue("@c_empshift", shifts);
                cmd.Parameters.AddWithValue("@c_empimg", emp.c_empimg);
                cmd.Parameters.AddWithValue("@c_empdepartment", deptId);
                cmd.Parameters.AddWithValue("@name", "savan");

                cmd.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
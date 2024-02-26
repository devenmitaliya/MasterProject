using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;
using Npgsql;
using NpgsqlTypes;


namespace mvc.Repositories
{
    public class UserRepositories : CommanRepository, IUserRepositories
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepositories(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Register(tblUser user)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            try
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO t_employeeusers(c_uname, c_uemail, c_password, c_role) VALUES(@c_uname, @c_uemail, @c_password, @c_role)";
                cmd.Parameters.AddWithValue("@c_uname", user.c_uname);
                cmd.Parameters.AddWithValue("@c_uemail", user.c_uemail);
                cmd.Parameters.AddWithValue("@c_password", user.c_password);
                cmd.Parameters.AddWithValue("@c_role", user.c_role);
                cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public int Login(tblUser user)
        {
            int rowCount = 0;
            string role = "";


            conn.Open();

            using (var command = new NpgsqlCommand("SELECT c_uid, c_uname, c_uemail, c_role, COUNT(*) FROM t_employeeusers WHERE c_uemail=@email AND c_password= @password GROUP BY c_uid, c_uname, c_uemail, c_role ", conn))
            {
                command.Parameters.Add("@email", NpgsqlDbType.Varchar).Value = user.c_uemail;
                command.Parameters.Add("@password", NpgsqlDbType.Varchar).Value = user.c_password;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        role = reader.GetString(3);
                        rowCount = reader.GetInt32(4);

                        var session = _httpContextAccessor.HttpContext.Session;
                        session.SetString("role", role);
                    }
                }

                // Console.WriteLine(HttpContext.Session.GetString("role"));
            }
            conn.Close();

            return rowCount;
        }
    }
}
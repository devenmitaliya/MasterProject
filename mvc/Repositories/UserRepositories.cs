using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mvc.Models;

namespace mvc.Repositories
{
    public class UserRepositories:CommanRepository,IUserRepositories
    {
        public void Register(tblUser user)
        {
            NpgsqlCommand cmd = new NpgsqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT INTO t_employeeusers(c_uname, c_uemail, c_password, c_role) VALUES(@c_uname, @c_uemail, @c_password, @c_role)";
            cmd.Parameters.AddWithValue("@c_uname", user.c_uname);
            cmd.Parameters.AddWithValue("@c_uemail", user.c_uemail);
            cmd.Parameters.AddWithValue("@c_password", user.c_password);
            cmd.Parameters.AddWithValue("@c_role", user.c_role);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public tblUser Login(tblUser data)
        {
           NpgsqlCommand cmd = new NpgsqlCommand();


            cmd.Connection = conn;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT * FROM t_employeeusers WHERE c_uemail= @c_uemail AND c_password=@c_password";
            cmd.Parameters.AddWithValue("@c_uemail", data.c_uemail);
            cmd.Parameters.AddWithValue("@c_password", data.c_password);
            tblUser user = null;
            conn.Open();
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            if (dt.Rows.Count > 0)
            {
                user = (from DataRow dr in dt.Rows
                        select new tblUser()
                        {
                           
                            c_uname = dr["c_uname"].ToString(),
                            c_uemail = dr["c_uemail"].ToString(),
                            c_password = dr["c_password"].ToString(),
                            c_role = dr["c_role"].ToString(),
                        }).ToList().FirstOrDefault();
            }

            conn.Close();
            return user;
        }
    }
}
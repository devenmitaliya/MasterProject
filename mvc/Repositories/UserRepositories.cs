using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using mvc.Models;


namespace mvc.Repositories
{
    public class UserRepositories : CommanRepository, IUserRepositories
    {
          private readonly IHttpContextAccessor _httpContextAccessor;

       public UserRepositories(IHttpContextAccessor httpContextAccessor)
       {
        _httpContextAccessor=httpContextAccessor;
       }
       
       public void Login(string email, string password){

            try{
                conn.Open();
                using var cmd=new NpgsqlCommand("select * from  t_employeeusers where c_uemail=@c_uemail and c_password=@c_password;",conn);
                  cmd.Parameters.AddWithValue("@c_uemail", email);
                cmd.Parameters.AddWithValue("@c_password", password);   
                var dr=cmd.ExecuteReader();
                if (dr.Read())
            {
            var session=_httpContextAccessor.HttpContext.Session;
            session.SetString("c_role",dr["c_role"].ToString());
             session.SetString("c_uname",dr["c_uname"].ToString());
           session.SetInt32("c_uid",(int)dr["c_uid"]);
            }

                

            }catch(Exception e){
                Console.WriteLine(e.Message);
            }finally{
                conn.Close();
            }
        }


}
}
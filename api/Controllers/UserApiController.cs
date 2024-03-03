using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Repositories;
using Microsoft.AspNetCore.Http;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserRepositories _userRepositories;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiController(IUserRepositories userRepositories, IHttpContextAccessor httpContextAccessor)
        {
            _userRepositories = userRepositories;
              _httpContextAccessor = httpContextAccessor;
        }

        // [HttpPost("Login")] 
        // public IActionResult Login(tblLogin log)
        // {
        //     int result = _userRepositories.LoginWithApi(log);
        //     if (result > 0)
        //     {
        //         return Ok("Valid");
        //     }
        //     else
        //     {
        //         return BadRequest("Not Valid");
        //     }
        // }

        // [HttpPost("register")]
        // public IActionResult Register(tblUser user)
        // {
        //     _userRepositories.Register(user);
        //     return Ok(new { message = "User registered successfully" });
        // }



[HttpPost]
[Route("Login")]
public IActionResult Login(tblLogin user)
{
    int result = _userRepositories.LoginWithApi(user);
    if (result > 0)
    {
        var session = _httpContextAccessor.HttpContext.Session;

        string role = session.GetString("role");
        string username = session.GetString("username");

        if (!string.IsNullOrEmpty(role) && !string.IsNullOrEmpty(username)) // Add null checks
        {
            if (role == "Admin")
            {
                HttpContext.Session.SetString("email", user.c_uemail);
                HttpContext.Session.SetString("username", username);
                return Ok(new { status = "Admin", name = username, role = role, email = user.c_uemail });
            }
            else
            {
                HttpContext.Session.SetString("email", user.c_uemail);
                HttpContext.Session.SetString("username", username);
                return Ok(new { status = "User", name = username, role = role, email = user.c_uemail });
            }
        }
        else
        {
            return BadRequest("Role or username is null or empty");
        }
    }
    else
    {
        return BadRequest("Not Valid");
    }
}


        [HttpPost("register")]
        public IActionResult Register(tblUser user)
        {
            _userRepositories.Register(user);
            return Ok(new { message = "User registered successfully" });
        }
    }
}

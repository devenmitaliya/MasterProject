using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Repositories;

namespace API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
           private readonly IUserRepositories _userRepositories;

        public UserApiController(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        // [HttpPost("login")]
        // public IActionResult Login(tblUser user)
        // {
        //     int rowCount = _userRepositories.Login(user);
        //     if (rowCount == 1)
        //     {
        //         return Ok(new { message = "Login successful", role = user.c_role });
        //     }
        //     else
        //     {
        //         return BadRequest(new { message = "Invalid credentials" });
        //     }
        // }




[HttpPost]
        [Route("Login")]
        public IActionResult Login(tblLogin log)
        {
            int result = _userRepositories.LoginWithApi(log);
            if (result > 0)
            {
                return Ok("Valid");
            }
            else
            {
                return BadRequest("Not Valid");
            }
        }

        [HttpPost("register")]
        public IActionResult Register(tblUser user)
        {
            // user.c_role = "User";
            _userRepositories.Register(user);
            return Ok(new { message = "User registered successfully" });
        }
        
    }
}
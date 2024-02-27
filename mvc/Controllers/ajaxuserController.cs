using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Repositories;

namespace mvc.Controllers
{
    public class ajaxuserController : Controller
    {
        private readonly IUserRepositories _userRepositories;

        public ajaxuserController(IUserRepositories userRepositories)
        {
            _userRepositories = userRepositories;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login([FromBody] tblUser user)
        {
            int rowCount = _userRepositories.Login(user);
            if (rowCount == 1)
            {
                return Json(new { success = true, redirectUrl = Url.Action("Index", "MvcAjax") });
            }
            else
            {
                return Json(new { success = false, message = "Invalid credentials" });
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromBody] tblUser user)
        {
            user.c_role = "User";
            _userRepositories.Register(user);
            return Json(new { message = "Registration successful" });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using mvc.Models;
using mvc.Repositories;

namespace mvc.Controllers
{
    public class KendogridloginController : Controller
    {
        private readonly IUserRepositories _userRepo;

        public KendogridloginController(IUserRepositories userRepo)
        {
            _userRepo = userRepo;
        }

        public IActionResult Index1()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterUser([FromBody] tblUser user)
        {
            try
            {
                _userRepo.Register(user);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Login([FromBody] tblUser user)
        {
            try
            {
                int rowCount = _userRepo.Login(user);

                if (rowCount > 0)
                {
                    var role = HttpContext.Session.GetString("role");
                    var username = HttpContext.Session.GetString("username");

                    return Json(new { success = true, role = role, username = username });

                }
                else
                {
                    // Login failed
                    return Json(new { success = false, error = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

    }
}

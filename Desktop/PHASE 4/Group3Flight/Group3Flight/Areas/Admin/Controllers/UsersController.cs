using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult RightObligations()
        {
            return View();
        }
    }
}

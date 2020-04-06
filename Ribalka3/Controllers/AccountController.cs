using Microsoft.AspNetCore.Mvc;

namespace Ribalka3.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

    }
}
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class RolController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

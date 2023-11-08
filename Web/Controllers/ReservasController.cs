using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ReservasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

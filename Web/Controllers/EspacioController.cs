using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class EspacioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

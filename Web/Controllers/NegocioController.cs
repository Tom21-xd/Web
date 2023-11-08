using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

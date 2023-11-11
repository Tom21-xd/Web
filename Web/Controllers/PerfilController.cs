using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class PerfilController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

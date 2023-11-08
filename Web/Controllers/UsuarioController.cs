using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

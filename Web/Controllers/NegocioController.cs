using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar negocio")]
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

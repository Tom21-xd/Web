using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar espacios")]
    public class EspacioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

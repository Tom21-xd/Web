using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class AgendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

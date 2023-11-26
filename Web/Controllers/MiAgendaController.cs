using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class MiAgendaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

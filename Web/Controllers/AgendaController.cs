using Microsoft.AspNetCore.Mvc;
using Web.Data;

namespace Web.Controllers
{
    public class AgendaController : Controller
    {
        Procedimientos cn= new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.usua=cn.obtenerUsuarios();
            return View();
        }
    }
}

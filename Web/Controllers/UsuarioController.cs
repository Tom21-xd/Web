using Microsoft.AspNetCore.Mvc;
using Web.Data;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.usuarios=cn.obtenerUsuarios();
            return View();
        }
    }
}

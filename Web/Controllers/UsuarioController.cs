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
            ViewBag.lt = cn.Listar("LISTARTIPODOC");
            ViewBag.lg = cn.Listar("LISTARGEN");
            ViewBag.r = cn.obtenerRoles();

            return View();
        }

        [HttpPost]
        public ActionResult editar_crear()
        {

            return RedirectToAction("Index","Usuario");
        }
    }
}

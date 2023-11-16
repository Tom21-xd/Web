using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar espacios")]
    public class EspacioController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.espacios = cn.obtenerEspacios();
            ViewBag.ubicaciones = cn.obtenerUbicaciones();
            return View();
        }


        public ActionResult crearEspacio(String nombre, bool estado, String descripcion, int ubicacion)
        {
            cn.crearEspacio(nombre, estado, descripcion, ubicacion);
            return RedirectToAction("Index", "Espacio");
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class UbicacionController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.pisos = cn.obtenerPisos();
            ViewBag.bloques = cn.obtenerBloques();
            ViewBag.ubicaciones = cn.obtenerUbicaciones();

            return View();
        }

        public ActionResult crearPiso(String nombre)
        {
            cn.crearPiso(nombre);
            return RedirectToAction("Index", "Ubicacion");
        }


        public ActionResult crearBloque(String nombre)
        {
            cn.crearBloque(nombre);
            return RedirectToAction("Index", "Ubicacion");
        }


        public ActionResult crearUbicacion(int piso, int bloque)
        {

            cn.crearUbicacion(piso, bloque);
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult eliminarPiso(int id)
        {
            bool aux = cn.eliminarPiso(id);
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult eliminarBloque(int id)
        {
            bool aux = cn.eliminarBloque(id);
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return RedirectToAction("Index", "Ubicacion");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Web.Data;

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

        /*
        [HttpPost]
        public ActionResult contarPisos(string numPisos)
        {
            int n = cn.contarPisos();
            return RedirectToAction("Index", "Ubicacion");
        }

        [HttpPost]
        public ActionResult contarBloques()
        {
            int n = cn.contarBloques();
            return RedirectToAction("Index", "Ubicacion");
        }*/
    }
}

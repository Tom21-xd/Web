using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar ubicacion")]
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

        public ActionResult crearPisos(String[] nombres)
        {
            foreach (var nombre in nombres)
            {
                cn.crearPiso(nombre);
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult crearBloques(String[] nombres)
        {
            foreach (var nombre in nombres)
            {
                cn.crearBloque(nombre);
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult crearUbicaciones(int[] pisos, int[] bloques, bool[] estados)
        {
            for (int i = 0; i < pisos.Length; i++)
            {
                cn.crearUbicacion(pisos[i], bloques[i], estados[i]);
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult editarUbicaciones(int[] id, bool[] estados)
        {
            for (int i = 0; i < id.Length; i++)
            {
                cn.editarUbicacion(id[i], estados[i]);
                
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult eliminarPisos(string[] nombres)
        {
            foreach (var nombre in nombres)
            {
                bool aux = cn.eliminarPiso(nombre);
                if (!aux)
                {
                    ViewData["Mensaje"] = "Hubo un problema";
                }
            }
            return RedirectToAction("Index", "Ubicacion");
        }

        public ActionResult eliminarBloques(string[] nombres)
        {
            foreach (var nombre in nombres)
            {
                bool aux = cn.eliminarBloque(nombre);
                if (!aux)
                {
                    ViewData["Mensaje"] = "Hubo un problema";
                }
            }
            return RedirectToAction("Index", "Ubicacion");
        }
    }
}

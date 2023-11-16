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

        [HttpPost]
        public ActionResult crearEspacio()
        {
            String ubicacion = Request.Form["selectCrearUbicacionEspacio"];
            ubicacion = ubicacion.Replace("Bloque", "");
            ubicacion = ubicacion.Replace("Piso", "");
            ubicacion = ubicacion.Replace(" ", "");
            int blo = 0;
            int piso = 0;
            for (int i = 0; i < ubicacion.Length; i++)
            {
                if (ubicacion[i] == ',')
                {
                    blo = int.Parse(ubicacion.Substring(0, i));
                    piso = int.Parse(ubicacion.Substring(i + 1, ubicacion.Length - 1 - i));
                    break;
                }
            }

            var ubi = cn.obtenerUbicaciones();
            int id = 0;
            foreach (UbicacionModel i in ubi)
            {
                if (i.Bloque == blo && i.Piso == piso)
                {
                    id = i.Id; break;
                }
            }
            
            cn.crearEspacio(Request.Form["inputCrearNombreEspacio"], Request.Form["textareaCrearDescripcionEspacio"], id);
            return RedirectToAction("Index", "Espacio");
        }

        [HttpPost]
        public ActionResult editarEspacio()
        {
            int id = int.Parse(Request.Form["inputEditarIdEspacio"]);
            String nombre = Request.Form["inputEditarNombreEspacio"];
            bool estado = Request.Form["selectEditarEstadoEspacio"] == "Activo";
            String descripcion = Request.Form["textareaEditarDescripcionEspacio"];
            String ubicacion = Request.Form["selectEditarUbicacionEspacio"];
            ubicacion = ubicacion.Replace("Bloque", "");
            ubicacion = ubicacion.Replace("Piso", "");
            ubicacion = ubicacion.Replace(" ", "");
            int blo = 0;
            int piso = 0;
            for (int i = 0; i < ubicacion.Length; i++)
            {
                if (ubicacion[i] == ',')
                {
                    blo = int.Parse(ubicacion.Substring(0, i));
                    piso = int.Parse(ubicacion.Substring(i + 1, ubicacion.Length - 1 - i));
                    break;
                }
            }

            var ubi = cn.obtenerUbicaciones();
            int id_ubicacion = 0;
            foreach (UbicacionModel i in ubi)
            {
                if (i.Bloque == blo && i.Piso == piso)
                {
                    id_ubicacion = i.Id; break;
                }
            }
            
            cn.editarEspacio(id, nombre, estado, descripcion, id_ubicacion);
            return RedirectToAction("Index", "Espacio");
        }

        [HttpPost]
        public ActionResult eliminarEspacio()
        {
            bool aux = cn.eliminarEspacio(int.Parse(Request.Form["selectEliminarEspacio"]));
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return RedirectToAction("Index", "Espacio");
        }
    }
}

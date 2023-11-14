using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize(Roles = "gestionar roles")]
    public class RolController : Controller
    {
        Procedimientos cn= new Procedimientos();
        public IActionResult Index()
        {
            List <UsuarioModel> oUsuarios = cn.obtenerUsuarios();
            List <RolModel> Oroles= cn.obtenerRoles();
            List<PermisoModel> Opermisos = cn.obtenerPermisos();
            

            ViewBag.usuarios = oUsuarios;
            ViewBag.roles= Oroles;
            ViewBag.permisos = Opermisos;

            return View();
        }

        [HttpPost]
        public ActionResult CrearRol(String nombre)
        {
            cn.CrearRol(nombre);
            return RedirectToAction("Index","Rol");
        }

        [HttpPost]
        public ActionResult eliminarRol(int id)
        {
            bool aux=cn.EliminarRol(id);
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return RedirectToAction("Index", "Rol");
        }

        [HttpPost]
        public ActionResult editarRol()
        {
            cn.editarRol(Request.Form["nombre"], ((Request.Form["select"] =="Activo")?1:0), Convert.ToInt32(Request.Form["idr"]));
            return RedirectToAction("Index", "Rol");
        }

        [HttpPost]
        public ActionResult agregarPermiso(String select)
        {
            bool aux = cn.agregarPermiso(Convert.ToInt32(Request.Form["idrol"]), select);
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }

            return RedirectToAction("Index", "Rol");
        }

        [HttpPost]
        public ActionResult eliminarPermiso(String select)
        {
            bool aux = cn.eliminarPermiso(Convert.ToInt32(Request.Form["idrol"]), select);
            if (!aux)
            {
                ViewData["Mensaje"] = "Hubo un problema";
            }

            return RedirectToAction("Index", "Rol");
        }
    }
}

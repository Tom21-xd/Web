using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles ="gestionar servicios")]
    public class ServiciosController : Controller
    {
        Procedimientos cn= new Procedimientos ();
        public IActionResult Index()
        {
            ViewBag.servicio = cn.obtenerservicios();
            return View();
        }
        [HttpPost]
        public ActionResult editarservicio()
        {
            ServicioModel ser = new ServicioModel() {
                Id = Convert.ToInt32(Request.Form["ids"]),
                Nombre = Request.Form["nombreser"],
                valor = Convert.ToSingle(Request.Form["valor"]),
                descripcion = Request.Form["desc"],
                Estado = ((Request.Form["seservi"] =="Activo")?true:false)
            };
            bool aux = cn.editarservicio(ser);
            if (!aux)
            {
                ViewData["mensaje"] = "Ha ocurrido un error";
            }
            return RedirectToAction("Index","Servicios");
        }

        [HttpPost]
        public ActionResult crearservicio()
        {
            ServicioModel ser = new ServicioModel()
            {
                Nombre = Request.Form["nombreser"],
                valor = Convert.ToSingle(Request.Form["valor"]),
                descripcion = Request.Form["desc"],
            };
            bool aux = cn.crearservicio(ser);
            if (!aux)
            {
                ViewData["mensaje"] = "Ha ocurrido un error";
            }
            return RedirectToAction("Index", "Servicios");

        }
    }
}

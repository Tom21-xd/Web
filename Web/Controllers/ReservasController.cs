using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar reservas")]
    public class ReservasController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.reservas = cn.obtenerReservas();
            ViewBag.servicios = cn.obtenerservicios();
            ViewBag.usuarios = cn.obtenerUsuarios();
            ViewBag.estados = cn.obtenerEstadosReservas();
            return View();
        }

        [HttpPost]
        public ActionResult obtenerAgendaFecha(string nombre1, string apellido1, string fecha)
        {
            return Json(cn.obtenerAgendaFecha(nombre1, apellido1, fecha));
        }
    }
}

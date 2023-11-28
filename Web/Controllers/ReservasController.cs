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
        public ActionResult obtenerAgendaFecha(int id, string fecha)
        {
            return Json(cn.obtenerAgendaFecha(id, fecha));
        }
        [HttpPost]
        public ActionResult editarReserva()
        {
            bool a = cn.cambiarReserva(Convert.ToInt32(Request.Form["reservaid"] +"") , Convert.ToInt32(Request.Form["selectEditarHoraReserva"] +"") , Request.Form["selectEditarEstadoReserva"]);
            return RedirectToAction("Index", "Reservas");
        }
    }
}

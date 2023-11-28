using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{

    public class ReservasController : Controller
    {
        Procedimientos cn = new Procedimientos();


        [Authorize(Roles = "gestionar reservas")]
        public IActionResult Index()
        {
            ViewBag.reservas = cn.obtenerReservas();
            ViewBag.servicios = cn.obtenerservicios();
            ViewBag.usuarios = cn.obtenerUsuarios();
            ViewBag.estados = cn.obtenerEstadosReservas();
            return View();
        }


        public IActionResult crearReservaServicio()
        {
            if (User.Identity.IsAuthenticated)
            {
                string nombreUsuario = User.Identity.Name;
                ViewBag.usuario = cn.obtenerUsua(nombreUsuario);
            }
            ViewBag.servicios = cn.obtenerservicios();
            return View();
        }

        public IActionResult MisReservas()
        {
            if (User.Identity.IsAuthenticated)
            {
                string nombreUsuario = User.Identity.Name;
                ViewBag.reservas = cn.obtenerReservasUsuario(nombreUsuario);
            }
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

        public ActionResult obtenerFechas(int id_empleado, int id_servicio)
        {
            return Json(cn.obtenerFechas(id_empleado, id_servicio));
        }

        public ActionResult obtenerEmpleadosServicio(int id_servicio)
        {
            return Json(cn.obtenerEmpleadosServicio(id_servicio));
        }

        public void crearReserva(int id_usuario, int id_empleado, string fechaReserva, string horaReserva)
        {
            cn.crearReserva(id_usuario, id_empleado, fechaReserva, horaReserva);
        }
    }
}

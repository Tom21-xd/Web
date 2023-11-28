﻿using Microsoft.AspNetCore.Authorization;
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
                ViewBag.usuario = cn.obtenerUsua(nombreUsuario);
            }
            return View();
        }

        [HttpPost]
        public ActionResult obtenerAgendaFecha(string nombre1, string apellido1, string fecha)
        {
            return Json(cn.obtenerAgendaFecha(nombre1, apellido1, fecha));
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
            //return RedirectToAction("MisReservas", "reservas");
        }
    }
}

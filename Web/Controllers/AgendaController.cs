﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar agenda")]
    public class AgendaController : Controller
    {
        Procedimientos cn= new Procedimientos();
        public IActionResult Index()
        {
            ViewBag.usua=cn.obtenerUsuarios();
            return View();
        }
        [HttpPost]
        public ActionResult agregarParametros(int cedula)
        {
            ProgramacionAgendaModel pagenda=new ProgramacionAgendaModel()
            {
                FechaInicio = Request.Form["fechaInicio"],
                FechaFin = Request.Form["fechaFin"],
                HoraInicio = Request.Form["horaInicio"],
                HoraFin = Request.Form["horaFin"],
                Duracion =Convert.ToInt32( Request.Form["duracion"]),
                lunes = ((Request.Form["L"]=="L")?true:false),
                martes = ((Request.Form["M"] == "M") ? true : false),
                miercoles = ((Request.Form["X"] == "X") ? true : false),
                jueves = ((Request.Form["J"] == "J") ? true : false),
                viernes = ((Request.Form["V"] == "V") ? true : false),
                sabado = ((Request.Form["S"] == "S") ? true : false),
                domingo = ((Request.Form["D"] == "D") ? true : false)

            };
            Console.WriteLine(pagenda);
            cn.agregarParametros(pagenda, cedula);
            return RedirectToAction("Index","Agenda");
        }
    }
}

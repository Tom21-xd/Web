using Microsoft.AspNetCore.Authorization;
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
                Dias = Request.Form["L"] + Request.Form["M"] + Request.Form["X"]+ Request.Form["J"]+ Request.Form["V"]+ Request.Form["S"]+ Request.Form["D"]

            };
            Console.WriteLine(pagenda);
            cn.agregarparametros(pagenda, cedula);
            return RedirectToAction("Index","Agenda");
        }
    }
}

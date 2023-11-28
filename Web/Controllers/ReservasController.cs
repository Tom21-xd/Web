using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{

    public class ReservasController : Controller
    {
        Procedimientos cn = new Procedimientos();
        private readonly IConverter _converter;


        public ReservasController(IConverter converter)
        {
            _converter = converter;
        }

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

        public IActionResult VistaParaPDF()
        {
            ViewBag.reserva1 = cn.obtenerReservas();
            return View();
        }
        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Reservas/VistaParaPDF";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects =
                {
                    new ObjectSettings()
                    {
                        Page=url_pagina
                    }
                }
            };
            var archivoPDF = _converter.Convert(pdf);
            string nombrePDF = "reporte_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";


            return File(archivoPDF, "application/pdf", nombrePDF);
        }
        public IActionResult DescargarEXCEL()
        {
            DataTable tablareservas = cn.obtenerReservas1();

            using (var libro = new XLWorkbook())
            {
                tablareservas.TableName = "Reservas";
                var hoja = libro.Worksheets.Add(tablareservas);
                hoja.ColumnsUsed().AdjustToContents();
                using (var memoria = new MemoryStream())
                {
                    libro.SaveAs(memoria);
                    var nombreExcel = string.Concat("Reporte ", DateTime.Now.ToString(), ".xlsx");
                    return File(memoria.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                }
            }
        }
    }
}

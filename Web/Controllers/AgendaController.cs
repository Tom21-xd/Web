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
    public class AgendaController : Controller
    {
        Procedimientos cn = new Procedimientos();
        private readonly IConverter _converter;

        public AgendaController(IConverter converter)
        {
            _converter = converter;

        }

        [Authorize(Roles = "gestionar agendas")]
        public IActionResult Index()
        {
            ViewBag.usua=cn.obtenerUsuarios();
            return View();
        }
        [Authorize(Roles = "gestionar mi agenda")]
        public IActionResult MiAgenda()
        {
            UsuarioModel a = cn.obtenerUsua(User.Identity.Name);
            ViewBag.usua1 = a;
            ViewBag.agenda = cn.agenda(a.persona.Id);
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
            if(User.IsInRole("gestionar mi agenda"))
            {
                return RedirectToAction("MiAgenda", "Agenda");
            }
            return RedirectToAction("Index", "Agenda");
        }

        [HttpPost]
        public ActionResult agendaEmple(int cedula)
        {
            return Json(cn.agenda(cedula));
        }

        public IActionResult VistaParaPDF()
        {
            
            return View();
        }
        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Agenda/VistaParaPDF";

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
            DataTable tablaAgenda = cn.agenda1(cn.obtenerUsua(User.Identity.Name).persona.Id);

            using (var libro = new XLWorkbook())
            {
                tablaAgenda.TableName = "Agenda";
                var hoja = libro.Worksheets.Add(tablaAgenda);
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

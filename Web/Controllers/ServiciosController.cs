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
    public class ServiciosController : Controller
    {
        private readonly IConverter _converter;
        Procedimientos cn = new Procedimientos ();

        public ServiciosController(IConverter converter)
        {
            _converter = converter;
        }

        [Authorize(Roles = "gestionar servicios")]
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

        public IActionResult VistaParaPDF()
        {
            ViewBag.servicio1 = cn.obtenerservicios();
            return View();
        }
        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Servicios/VistaParaPDF";

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
            DataTable tablaservicio = cn.obtenerservicios1();

            using (var libro = new XLWorkbook())
            {
                tablaservicio.TableName = "Servicios";
                var hoja = libro.Worksheets.Add(tablaservicio);
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

using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Policy;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar usuarios")]
    public class UsuarioController : Controller
    {
        private readonly IConverter _converter;
        Procedimientos cn = new Procedimientos();

        public UsuarioController (IConverter converter)
        {
            _converter = converter;
        }

        public IActionResult Index()
        {
            ViewBag.usuarios=cn.obtenerUsuarios();
            ViewBag.lt = cn.Listar("LISTARTIPODOC");
            ViewBag.lg = cn.Listar("LISTARGEN");
            ViewBag.r = cn.obtenerRoles();

            return View();
        }

        [HttpPost]
        public ActionResult editar_usuario()
        {
            PersonaModel persona = new PersonaModel();
            persona.Id = Convert.ToInt32(Request.Form["cedula"] + "");
            persona.Nombre1 = Request.Form["nombre1"];
            persona.Nombre2 = Request.Form["nombre2"];
            persona.Apellido1 = Request.Form["apellido1"];
            persona.Apellido2 = Request.Form["apellido2"];
            persona.FechaNacimiento = Request.Form["fechanaci"];
            persona.Telefono = Request.Form["telefono"] + "-" + Request.Form["telefono1"];
            persona.Direccion = "";
            persona.tipodoc = Request.Form["tipodoc"];
            persona.genero = Request.Form["genero"];
            UsuarioModel usuario = new UsuarioModel();
            usuario.Nombre = Request.Form["nombreusuario"]; 
            usuario.Correo = Request.Form["correo"];
            usuario.rol = new RolModel()
            {
                Nombre= Request.Form["rol"]
            };
            usuario.Estado = ((Request.Form["estado"] == "Activo") ? true : false);
            usuario.persona = persona;
            bool aux=cn.ActualizarUsuario(usuario);
            if (!aux)
            {
                ViewData["mensaje"] = "Ha ocurrido un error";
            }
            return RedirectToAction("Index","Usuario");
        }

        [HttpPost]
        public ActionResult crear_usuario()
        {
            PersonaModel persona = new PersonaModel();
            persona.Id = Convert.ToInt32(Request.Form["cedula"] + "");
            persona.Nombre1 = Request.Form["nombre1"];
            persona.Nombre2 = Request.Form["nombre2"];
            persona.Apellido1 = Request.Form["apellido1"];
            persona.Apellido2 = Request.Form["apellido2"];
            persona.FechaNacimiento = Request.Form["fechanaci"];
            persona.Telefono = Request.Form["telefono"] + "-" + Request.Form["telefono1"];
            persona.Direccion = "";
            persona.tipodoc = Request.Form["tipodoc"];
            persona.genero = Request.Form["genero"];
            UsuarioModel usuario = new UsuarioModel();
            usuario.Nombre = Request.Form["nombreusuario"];
            usuario.Correo = Request.Form["correo"];
            usuario.Contrasenia = persona.Id+"";
            usuario.persona = persona;
            cn.Registrar(usuario);
            return RedirectToAction("Index", "Usuario");
        }

        public IActionResult VistaParaPDF()
        {
            ViewBag.usuarios = cn.obtenerUsuarios();
            return View();
        }
        public IActionResult MostrarPDFenPagina()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Usuario/VistaParaPDF";

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings= new GlobalSettings()
                {
                    PaperSize=PaperKind.A4,
                    Orientation=Orientation.Portrait
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
   
            return File(archivoPDF,"application/pdf" );
        }

        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Usuario/VistaParaPDF";

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
            string nombrePDF = "reporte_" + DateTime.Now.ToString("ddMMyyyyHHmmss")+".pdf";


            return File(archivoPDF, "application/pdf", nombrePDF);
        }

    }
}

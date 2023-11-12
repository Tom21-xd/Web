using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        Procedimientos cn = new Procedimientos();
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
    }
}

using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class PerfilController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string nombreUsuario = User.Identity.Name;

                ViewBag.usuario = cn.obtenerUsua(nombreUsuario);
            }
            return View();
        }

        [HttpPost]
        public ActionResult editarPerfil()
        {
            PersonaModel persona = new PersonaModel();

            persona.Id = Convert.ToInt32(Request.Form["inputNumeroDocumento"]);
            persona.Nombre1 = Request.Form["inputNombre1"];
            persona.Nombre2 = Request.Form["inputNombre2"];
            persona.Apellido1 = Request.Form["inputApellido1"];
            persona.Apellido2 = Request.Form["inputApellido2"];
            persona.FechaNacimiento = Request.Form["inputFechaNacimiento"];
            persona.Telefono = Request.Form["inputCelular1"] + "-" + Request.Form["inputCelular2"];
            persona.Direccion = "";
            persona.tipodoc = Request.Form["selectTipoDocumento"];
            persona.genero = Request.Form["selectGenero"];
            UsuarioModel usuario = new UsuarioModel();
            usuario.Nombre = Request.Form["inputName"];
            usuario.Correo = Request.Form["inputCorreo"];
            usuario.Contrasenia = Request.Form["inputContrasenia"];
            usuario.persona = persona;
            cn.editarPerfil(usuario);
            return RedirectToAction("Index", "Perfil");
        }
    }
}

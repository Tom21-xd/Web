
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using NuGet.Protocol.Plugins;
using System.Configuration;
using Web.Data;
using Web.Helpers;
using Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Web.Controllers{
    public class AccesoController : Controller{

        Procedimientos cn = new Procedimientos();

        public IActionResult Login(){
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Inicio", "Home");
            }
            return View();
        }

        public IActionResult Registro(){
            ViewBag.lt = cn.Listar("LISTARTIPODOC");
            ViewBag.lg = cn.Listar("LISTARGEN");
            return View();
        }

        public IActionResult OlvContrasenia(){
            List<string> a= cn.Listar("LISTARTIPODOC");
            return View(a);
        }

        [HttpPost]
        public ActionResult Registrarse(){
            PersonaModel persona = new PersonaModel();
            persona.Id= Convert.ToInt32(Request.Form["id"]+"");
            persona.Nombre1 = Request.Form["nombre1"];
            persona.Nombre2 = Request.Form["nombre2"];
            persona.Apellido1 = Request.Form["apellido1"];
            persona.Apellido2 = Request.Form["apellido2"];
            persona.FechaNacimiento = Request.Form["fechanaci"];
            persona.Telefono = Request.Form["celular"]+Request.Form["2docelular"];
            persona.Direccion = "";
            persona.tipodoc = Request.Form["tipodoc"];
            persona.genero = Request.Form["genero"];
            UsuarioModel usuario = new UsuarioModel();
            usuario.Nombre = "";
            usuario.Correo = Request.Form["correo"];
            usuario.Contrasenia = Request.Form["contra"];
            usuario.persona= persona;
            cn.Registrar(usuario);
            return RedirectToAction("Login", "Acceso"); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(String usuario_correo,String Contrasenia){
            UsuarioModel ousuario=cn.validar(usuario_correo,Contrasenia);
            if (ousuario.Nombre != null){
                 
                 var claims = new List<Claim>
                 {
                    new Claim(ClaimTypes.Name, ousuario.Nombre),
                    new Claim("Correo", ousuario.Correo),
                    
                 };
                
                 foreach (PermisoModel rol in ousuario.rol.permisos)
                 {
                    claims.Add(new Claim(ClaimTypes.Role, rol.Nombre));
                 }
                

                claims.Add(new Claim("Rol", ousuario.rol.Nombre));
                
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
               
                return RedirectToAction("Inicio", "Home");
            }else{
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecuContra(int cedula){
            ServicioEmail aux = new ServicioEmail();
            String nuevac = generarclave();
            String correo = cn.RecuContra(cedula, nuevac);
            aux.SendEmailGmail(correo, "Recuperacion De Contraseña", "Su nueva Contraseña es :" + nuevac);
            return View("Login");
        }

        public String generarclave(){
            Random aleatorio = new Random();
            string conjuntoCaracteres = "abcdefghijklmnopqrstuvwxyz0123456789";
            string cadena = "";
            for (int i = 0; i < 6; i++){
                cadena += conjuntoCaracteres[aleatorio.Next(conjuntoCaracteres.Length)];
            }
            return cadena;
        }

        public async Task<IActionResult> salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }
    }
}

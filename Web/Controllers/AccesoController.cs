
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI;
using NuGet.Protocol.Plugins;
using Org.BouncyCastle.Crypto.Tls;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Web.Data;
using Web.Models;

namespace Web.Controllers{
    public class AccesoController : Controller{

        Procedimientos cn = new Procedimientos();

        

        public IActionResult Login(){
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
            Persona persona = new Persona();
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
        public ActionResult Login(String usuario_correo,String Contrasenia){
            UsuarioModel ousuario=cn.validar(usuario_correo,Contrasenia);
            if (ousuario.Nombre != null){
                TempData["Nombre"] = ousuario.Nombre;
                return RedirectToAction("Inicio", "Home");
            }else{
                ViewData["Mensaje"] = "Hubo un problema";
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecuContra(int cedula){

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
    }
}

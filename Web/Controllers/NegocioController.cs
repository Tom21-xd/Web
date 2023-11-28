using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Models;

namespace Web.Controllers
{
    [Authorize (Roles = "gestionar negocio")]
    public class NegocioController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.neg = JsonConvert.DeserializeObject<NegocioModel>(System.IO.File.ReadAllText("Data/Negocio.json"));
            return View();
        }

        [HttpPost]
        public ActionResult GuardarDatos()
        {
            
            NegocioModel neg = new NegocioModel() {
                Nombre = Request.Form["nombre"],
                Celular=Request.Form["celular"],
                Correo = Request.Form["correo"],
                Direccion= Request.Form["direccion"],
                Telefono =Request.Form["telefono"]
            };
            try
            {
                var json = JsonConvert.SerializeObject(neg);
                System.IO.File.WriteAllText("Data/Negocio.json", json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("Index","Negocio");
        }
    }
}

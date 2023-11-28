using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers{

    [Authorize]
    public class HomeController : Controller{
        public IActionResult Inicio(){
            
            return View();
        }

        public IActionResult CentroDeAyuda()
        {

            return View();
        }
    }


}

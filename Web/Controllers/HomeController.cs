using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers{
    public class HomeController : Controller{
        public IActionResult Inicio(){
            
            return View();
        }
    }
}

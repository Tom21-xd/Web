using Microsoft.AspNetCore.Mvc;
using Web.Data;
using Web.Models;

namespace Web.Controllers
{
    public class RolController : Controller
    {
        Procedimientos cn= new Procedimientos();
        public IActionResult Index()
        {
            List <UsuarioModel> oUsuarios = cn.obtenerUsuarios();
            List <RolModel> Oroles= cn.obtenerRoles();
            List<PermisoModel> Opermisos = cn.obtenerPermisos();

            ViewBag.usuarios = oUsuarios;
            ViewBag.roles= Oroles;
            ViewBag.permisos = Opermisos;

            return View();
        }
    }
}

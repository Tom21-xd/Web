﻿using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class UsuarioController : Controller
    {
        Procedimientos cn = new Procedimientos();
        public IActionResult Index()
        {
            return View();
        }
    }
}

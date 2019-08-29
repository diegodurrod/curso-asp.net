using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sistem_Ventas.Library;

namespace Sistem_Ventas.Areas.Principal.Controllers
{
    [Authorize]
    [Area("Principal")]
    public class PrincipalController : Controller
    {
        private LUsuarios _usuarios;

        public PrincipalController()
        {
            _usuarios = new LUsuarios();
        }

        public IActionResult Index()
        {
            ViewData["Roles"] = _usuarios.UserData(HttpContext); // Pasa la variable a la vista
            return View();
        }
    }
}
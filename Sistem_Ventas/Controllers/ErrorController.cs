using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sistem_Ventas.Controllers
{
    // Anotación de rutas
    //[Route("/Error")]
    public class ErrorController : Controller
    {
        // Si no le indicamos el método de solicitud de este método de esta misma clase, por defecto, se solicitará por GET
        public IActionResult Error(int? statusCode = null)
        {
            // HasValue devolverá true si tiene algún valor y no es nulo
            if(statusCode.HasValue)
            {
                if(statusCode.Value == 404 || statusCode.Value == 500)
                {
                    // Asi pasamos información de nuestro controlador a la vista Error
                    ViewData["Error"] = statusCode.ToString();
                    ViewData["Mensaje"] = "Toma error";
                }
            }
            return View();
        }
    }
}
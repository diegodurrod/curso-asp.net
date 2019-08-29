using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sistem_Ventas.Areas.Principal.Controllers;
using Sistem_Ventas.Models;

namespace Sistem_Ventas.Controllers
{
    public class HomeController : Controller
    {
        private Library.LUsuarios _usuarios;
        private SignInManager<IdentityUser> _signInManager;

        IServiceProvider _serviceProvider;
        //public HomeController(IServiceProvider serviceProvider)
        public HomeController(IServiceProvider serviceProvider,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _serviceProvider = serviceProvider;

            _signInManager = signInManager;
            _usuarios = new Library.LUsuarios(userManager, signInManager, roleManager);
            //EjecutarTarea();
        }
        public async Task<IActionResult> Index()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(PrincipalController.Index), "Principal");
            }

            await CreateRoles(_serviceProvider);
            return View();
            
        }

        // Con la anotación siguiente le indicamos que se ejecute sólamente cuando se solicite por POST
        [HttpPost]
        // Con la anotación siguiente, le indicamos que pueda acceder cualquier usuario no identificado
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModels model)
        {
            // Verificamos que el Email y Password son correctos
            if(ModelState.IsValid)
            {
                List<object[]> listObject = await _usuarios.userLogin(model.Input.Email, model.Input.Password);
                object[] objects = listObject[0];
                var _identityError = (IdentityError)objects[0];
                model.ErrorMessage = _identityError.Description;

                if(model.ErrorMessage.Equals("True"))
                {
                    var data = JsonConvert.SerializeObject(objects[1]);
                    HttpContext.Session.SetString("User", data);
                    return RedirectToAction(nameof(PrincipalController.Index), "Principal");
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            String mensaje;
            try
            {
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                // Iniciamos la colección
                String[] rolesName = { "Admin", "User" };
                foreach (var item in rolesName)
                {
                    // Comprobamos que existe los roles de rolesName
                    var rolesExist = await roleManager.RoleExistsAsync(item);
                    if (!rolesExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(item));
                    }
                }
                
                var user = await userManager.FindByIdAsync("ddc84604-2d6c-4ce7-a45b-90b217ebcf6b");
                await userManager.AddToRoleAsync(user, "Admin");
            }
            catch(Exception ex)
            {
                mensaje = ex.Message;
            }


        }

        private async void EjecutarTarea()
        {
            var data = await Tareas();

            String tarea = "Ahora ejecutaremos las demás líneas de código porque la tarea ha finalizado";
        }

        private async Task<String> Tareas()
        {
            Thread.Sleep(20 * 1000);
            String tarea = "Tarea finalizada";

            return tarea;
        }
    }
}

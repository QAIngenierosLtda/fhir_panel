using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using AspStudio.Models;
using AspStudio.Data;
using System.Security.Claims;
using System.Globalization;

namespace AspStudio.Controllers
{

    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationDbContext dbContext;

        public AccountController(ILogger<AccountController> logger, ApplicationDbContext _dbContext)
        {
            _logger = logger;
            dbContext = _dbContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        /*
         dbContext.Usuarios.Where(c=>c.Nombre.Contains("a")) // traer todos los registros cuyo nombre sea A

            // insertar un registro
            var usuario = new Usuario()
                {
                     Nombre = "Juan",
                     Login = "juanperez",
                     Password = "123"
                };

                dbContext.Usuarios.Add(std);
                cbContext.SaveChanges();

                En la clase Data/ApplicationDbContext se deben agregar los modelos para que se guarden la base de datos
                para usuario se adiciono

            public DbSet<Usuario> Usuarios { get; set; }
            en la clase Usuario encuentran como marcar los nombres de las tablas y las columnas


         */

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginInfo)
        {
           

            if (dbContext.Usuarios.Count(p => p.Login.Equals(loginInfo.Login) && p.Password.Equals(loginInfo.Password)) > 0)
            {
                Usuario usuario = dbContext.Usuarios.FirstOrDefault(p => p.Login.Equals(loginInfo.Login) && p.Password.Equals(loginInfo.Password));
                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(usuario), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await this.HttpContext.SignInAsync("fhirlogin", principal);
                Console.WriteLine("Autenticado...");

                if (usuario.FechaInicial <= DateTime.Now && usuario.FechaFinal >= DateTime.Now)
                {
                    MyGlobalVariables.isAuth = true;
                    if (usuario.Rol == 1)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Enrol");
                    }
                }
                else
                {
                    MyGlobalVariables.isAuth = false;
                    ViewBag.Mensaje = "Registro fuera de los l�mites de fecha";
                    return View();
                }

                


            }
            else return View();
        }
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();
            Console.WriteLine("Logout...");
            return RedirectToAction("Login", "Account");
        }

            private IEnumerable<Claim> GetUserClaims(Usuario _usuario)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _usuario.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, _usuario.Nombre));
            return claims;
        }

    }
}
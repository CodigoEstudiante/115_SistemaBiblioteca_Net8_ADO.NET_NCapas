using BibliotecaData.Contrato;
using BibliotecaEntidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BibliotecaWeb.Models.Dtos;

namespace BibliotecaWeb.Controllers
{
    public class AccesoController : Controller
    {
        private readonly IUsuarioRepositorio _repositorio;
        public AccesoController(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }
        public IActionResult Login()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            if (claimuser.Identity!.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(VMUsuarioLogin modelo)
        {
            if(modelo.NombreUsuario == null || modelo.Clave == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            Usuario usuario_encontrado = await _repositorio.Login(modelo.NombreUsuario, modelo.Clave);

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "No se encontraron coincidencias";
                return View();
            }

            ViewData["Mensaje"] = null;

            //aqui guarderemos la informacion de nuestro usuario
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, usuario_encontrado.NombreCompleto),
                    new Claim(ClaimTypes.NameIdentifier, usuario_encontrado.IdUsuario.ToString()),
                    new Claim(ClaimTypes.Role,"Admin")
                };


            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Denegado()
        {
            return View();
        }
    }
}

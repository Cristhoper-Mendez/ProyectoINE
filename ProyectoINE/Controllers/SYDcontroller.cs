
using Microsoft.AspNetCore.Mvc;
using ProyectoINE.Models;
using ProyectoINE.Services;

namespace ProyectoINE.Controllers
{
    public class SYDController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(DatosSYD datos)
        {
            var resultado = SYDService.Calcular(datos);

            if (resultado is { Error: not null })
            {
                ModelState.AddModelError("", ((dynamic)resultado).Error);
                return View(datos);
            }

            return View("SYDTable", resultado);
        }
    }
}

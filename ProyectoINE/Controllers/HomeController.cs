using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoINE.Models;
using ProyectoINE.Models.ViewModels;
using System.Diagnostics;

namespace ProyectoINE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Creamos el modelo para enviar a la vista
            var model = new TipoViewModel
            {
                Items = ObtenerTiposDeDepr()
            };

            return View(model);
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

        // Método para obtener los tipos de depreciación
        private List<SelectListItem> ObtenerTiposDeDepr()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Evaluar método óptimo" },
            new SelectListItem { Value = "2", Text = "Linea Recta" },
            new SelectListItem { Value = "3", Text= "Saldo Decreciente" },
            new SelectListItem { Value = "4", Text = "Digitos de los años" },
            new SelectListItem { Value = "5", Text = "Unidades de producción" }
        };
        }
    }
}

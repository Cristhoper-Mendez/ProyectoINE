using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoINE.Models;
using ProyectoINE.Models.ViewModels;
using System.Diagnostics;
using MathNet.Numerics.Statistics;

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

        public string CalcularDesviacionEstandarMasBaja(
            double[] Metodo1, 
            double[] Metodo2,
            double[] Metodo3, 
            double[] Metodo4)
        {
            // Validar que los arrays no sean nulos o vacíos
            if (Metodo1 == null || Metodo2 == null || Metodo3 == null || Metodo4 == null)
                throw new ArgumentNullException("Ningún array puede ser nulo.");
    
            if (Metodo1.Length == 0 || Metodo2.Length == 0 || Metodo3.Length == 0 || Metodo4.Length == 0)
                throw new ArgumentException("Ningún array puede estar vacío.");

            // Calcular desviaciones estándar
            double sd1 = Metodo1.StandardDeviation();
            double sd2 = Metodo2.StandardDeviation();
            double sd3 = Metodo3.StandardDeviation();
            double sd4 = Metodo4.StandardDeviation();

            // Determinar cuál es la menor
            double minSD = Math.Min(Math.Min(sd1, sd2), Math.Min(sd3, sd4));

            // Usar switch-case para identificar el método ganador
            string mensaje;
            switch (minSD)
            {
                case var _ when minSD == sd1:
                    mensaje = $"Método Depreciación por Línea Recta tiene la menor desviación estándar: {sd1:N2}";
                    break;
                case var _ when minSD == sd2:
                    mensaje = $"Método Depreciación por Saldo Decreciente tiene la menor desviación estándar: {sd2:N2}";
                    break;
                case var _ when minSD == sd3:
                    mensaje = $"Método Suma de los Dígitos de los Años (SYD) tiene la menor desviación estándar: {sd3:N2}";
                    break;
                case var _ when minSD == sd4:
                    mensaje = $"Método Unidad de Producción tiene la menor desviación estándar: {sd4:N2}";
                    break;
                default:
                    mensaje = "Error inesperado al comparar desviaciones.";
                    break;
            }

            return mensaje;
        }
     

        // M�todo para obtener los tipos de depreciaci�n
        private List<SelectListItem> ObtenerTiposDeDepr()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Evaluar m�todo �ptimo" },
            new SelectListItem { Value = "2", Text = "Linea Recta" },
            new SelectListItem { Value = "3", Text= "Saldo Decreciente" },
            new SelectListItem { Value = "4", Text = "Digitos de los a�os" },
            new SelectListItem { Value = "5", Text = "Unidades de producci�n" }
        };
        }
    }
}

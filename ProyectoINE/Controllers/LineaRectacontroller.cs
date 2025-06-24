using Microsoft.AspNetCore.Mvc;
using ProyectoINE.Models;

namespace ProyectoINE.Controllers
{
    public class LineaRectaController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(DatosLineaRecta datos)
        {
            if (datos.N <= 0 || datos.k <= 0 || datos.k > datos.N)
            {
                ModelState.AddModelError("", "Verifique que N y k sean válidos.");
                return View(datos);
            }

            double B = datos.B;
            double VR = datos.VR;
            int N = datos.N;
            int k = datos.k;

            // Cálculo de depreciación por línea recta
            double depreciacionAnual = (B - VR) / N;
            double depreciacionAcumulada = depreciacionAnual * k;
            double valorLibro = B - depreciacionAcumulada;

            ViewBag.Depreciacion = depreciacionAnual;
            ViewBag.DepreciacionAcumulada = depreciacionAcumulada;
            ViewBag.ValorLibro = valorLibro;

            return View(datos);
        }
    }
}

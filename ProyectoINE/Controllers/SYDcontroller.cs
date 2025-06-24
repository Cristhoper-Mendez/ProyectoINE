using Microsoft.AspNetCore.Mvc;
using ProyectoINE.Models;

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
            if (datos.N <= 0 || datos.k <= 0 || datos.k > datos.N)
            {
                ModelState.AddModelError("", "Verifique que N y k sean válidos.");
                return View(datos);
            }

            double B = datos.B;
            double VR = datos.VR;
            int N = datos.N;
            int k = datos.k;

            double d_k = (B - VR) * (2.0 * (N - k + 1)) / (N * (N + 1));
            double parte1 = (2.0 * (B - VR) / N) * k;
            double parte2 = ((B - VR) * k * (k + 1)) / (N * (N + 1));
            double VL_k = B - parte1 + parte2;
            double dAcumulada = B - VL_k;

            ViewBag.Depreciacion = d_k;
            ViewBag.ValorLibro = VL_k;
            ViewBag.DepreciacionAcumulada = dAcumulada;

            return View(datos);
        }
    }
}

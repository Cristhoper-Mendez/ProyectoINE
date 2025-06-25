using Microsoft.AspNetCore.Mvc;
using ProyectoINE.Models.ViewModels;

namespace ProyectoINE.Controllers
{
    public class LineaRectaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calcular(TipoViewModel model)
        {
            var resultado = new ResultadoLineaRectaViewModel();

            decimal costoInicial = model.CostoInicial;
            int vidaUtil = model.VidaUtilAnios;
            decimal valorRescate = model.ValorRescate;
            decimal depreciacionAnual = (costoInicial - valorRescate) / vidaUtil;
            decimal valorEnLibros = costoInicial;

            for (int periodo = 1; periodo <= vidaUtil; periodo++)
            {
                decimal deducibleFiscal = valorEnLibros * (model.ImpactoFiscal / 100m);
                decimal valorInicialPeriodo = valorEnLibros;
                valorEnLibros -= depreciacionAnual;

                if (valorEnLibros < valorRescate)
                    valorEnLibros = valorRescate;

                resultado.Filas.Add(new ResultadoLRectaViewModel.Fila
                {
                    NumeroPeriodo = periodo,
                    ValorInicial = valorInicialPeriodo,
                    Depreciacion = depreciacionAnual,
                    ValorFinal = valorEnLibros,
                    DeducibleFiscal = deducibleFiscal
                });
            }

            return View("Resultado", resultado);
        }
    }
}

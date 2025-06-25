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

        public ResultadoLineaRectaViewModel Calcular(TipoViewModel model)
        {
            var resultado = new ResultadoLineaRectaViewModel();

            decimal costoInicial = model.CostoInicial;
            int vidaUtil = model.VidaUtilAnios;
            decimal valorRescate = model.ValorResidual;
            decimal depreciacionAnual = (costoInicial - valorRescate) / vidaUtil;
            decimal valorEnLibros = costoInicial;

            for (int periodo = 1; periodo <= vidaUtil; periodo++)
            {
                decimal deducibleFiscal = costoInicial * (model.ImpactoFiscal / 100m);
                decimal valorInicialPeriodo = valorEnLibros;
                valorEnLibros -= depreciacionAnual;

                if (valorEnLibros < valorRescate)
                    valorEnLibros = valorRescate;

                resultado.Filas.Add(new ResultadoLineaRectaViewModel.Fila
                {
                    NumeroPeriodo = periodo,
                    ValorInicial = valorInicialPeriodo,
                    Depreciacion = depreciacionAnual,
                    ValorFinal = valorEnLibros,
                    DeducibleFiscal = deducibleFiscal
                });
            }

            return resultado;
        }

        public double[] CalcularDepreciaciones(TipoViewModel model)
        {
            decimal costoInicial = model.CostoInicial;
            int vidaUtil = model.VidaUtilAnios;
            decimal valorRescate = model.ValorResidual;
            decimal depreciacionAnual = (costoInicial - valorRescate) / vidaUtil;

            double[] depreciaciones = new double[vidaUtil];

            for (int periodo = 0; periodo < vidaUtil; periodo++)
            {
                depreciaciones[periodo] = (double)depreciacionAnual;
            }

            return depreciaciones;
        }

    }
}

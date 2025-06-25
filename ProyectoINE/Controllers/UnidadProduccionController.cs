using Microsoft.AspNetCore.Mvc;
using ProyectoINE.ViewModels;
using System.Diagnostics;

namespace ProyectoINE.Controllers
{
    public class UnidadProduccionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Calcular([FromBody] UnidadProduccionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { error = "Datos inválidos enviados al servidor." });
            }

            if (model.UnidadesEstimadas == 0)
            {
                return BadRequest(new { error = "Las unidades estimadas deben ser mayor a 0." });
            }

            if (model.VidaUtilAnios == 0)
            {
                return BadRequest(new { error = "La vida útil debe ser mayor a 0." });
            }

            decimal depreciacionPorUnidad = (model.CostoInicial - model.ValorRescate) / model.UnidadesEstimadas;
            decimal valorInicial = model.CostoInicial;
            int numeroPeriodo = 1;
            decimal unidadesPorPeriodo = model.UnidadesEstimadas / model.VidaUtilAnios;

            // Inicializa el resultado
            var resultado = new ResultadoUnidadProduccionViewModel
            {
                Filas = new List<ResultadoUnidadProduccionViewModel.Fila>()
            };

            while (numeroPeriodo <= model.VidaUtilAnios && valorInicial > model.ValorRescate)
            {
                decimal depreciacion = depreciacionPorUnidad * unidadesPorPeriodo;

                decimal valorFinalCalculado = valorInicial - depreciacion;
                if (valorFinalCalculado < model.ValorRescate)
                {
                    depreciacion = valorInicial - model.ValorRescate;
                    valorFinalCalculado = model.ValorRescate;
                }

                decimal valorFinal = valorFinalCalculado;
                decimal deducibleFiscal = valorInicial * (model.ImpactoFiscal / 100m);

                resultado.Filas.Add(new ResultadoUnidadProduccionViewModel.Fila
                {
                    NumeroPeriodo = numeroPeriodo,
                    DeducibleFiscal = deducibleFiscal,
                    ValorInicial = valorInicial,
                    Depreciacion = depreciacion,
                    ValorFinal = valorFinal,
                    UnidadesProducidas = (int)unidadesPorPeriodo
                });

                valorInicial = valorFinal;
                numeroPeriodo++;
            }

            return Json(resultado);
        }

        // Método para calcular las depreciaciones por unidades de producción
        // y devolver un array de valores de depreciación por periodo
        public double[] CalcularDepreciaciones(UnidadProduccionViewModel model)
        {
            List<double> depreciaciones = new();

            // Cálculo de la depreciación por unidad
            double depreciacionPorUnidad = (double)(model.CostoInicial - model.ValorRescate) / (double)model.UnidadesEstimadas;

            double valorInicial = (double)model.CostoInicial;
            double valorRescate = (double)model.ValorRescate;
            int vidaUtil = model.VidaUtilAnios;

            // Unidades por período (distribución uniforme)
            double unidadesPorPeriodo = (double)model.UnidadesEstimadas / vidaUtil;

            int numeroPeriodo = 1;

            while (numeroPeriodo <= vidaUtil && valorInicial > valorRescate)
            {
                double depreciacion = depreciacionPorUnidad * unidadesPorPeriodo;

                // Evitar valor menor al valor de rescate
                if (valorInicial - depreciacion < valorRescate)
                    depreciacion = valorInicial - valorRescate;

                depreciaciones.Add(depreciacion);
                valorInicial -= depreciacion;
                numeroPeriodo++;
            }

            return depreciaciones.ToArray();
        }

        // Método para calcular depreciación basada en unidades específicas producidas en un período
        public decimal CalcularDepreciacionPorUnidades(decimal costoInicial, decimal valorRescate,
            int unidadesEstimadas, int unidadesProducidas)
        {
            decimal depreciacionPorUnidad = (costoInicial - valorRescate) / unidadesEstimadas;
            return depreciacionPorUnidad * unidadesProducidas;
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using ProyectoINE.Models.ViewModels;
using System.Diagnostics;

namespace ProyectoINE.Controllers
{
    public class DecreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // Método para calcular la depreciación por saldo decreciente
        // Y enviar el resultado a la vista
        public ResultadoDecreViewModel Calcular(TipoViewModel model)
        {
            var resultado = new ResultadoDecreViewModel();

            decimal valorInicial = model.CostoInicial;
            int vidaUtil = model.VidaUtilAnios;
            int numeroPeriodo = 1;

            while (numeroPeriodo <= vidaUtil && valorInicial > 0)
            {
                decimal depreciacion = valorInicial * (2m / vidaUtil);

                // Para evitar depreciación que baje valor a negativo:
                if (depreciacion > valorInicial)
                    depreciacion = valorInicial;

                decimal valorFinal = valorInicial - depreciacion;
                // El Valor inicial por el porcentaje máximo de deducción fiscal.
                decimal deducibleFiscal = valorInicial * (model.ImpactoFiscal / 100m);

                resultado.Filas.Add(new ResultadoDecreViewModel.Fila
                {
                    NumeroPeriodo = numeroPeriodo,
                    DeducibleFiscal = deducibleFiscal,
                    ValorInicial = valorInicial,
                    Depreciacion = depreciacion,
                    ValorFinal = valorFinal
                });

                valorInicial = valorFinal;
                numeroPeriodo++;
            }

            return resultado;
        }

        // Método para calcular las depreciaciones por saldo decreciente
        // y devolver un array de valores de depreciación por periodo
        public double[] CalcularDepreciaciones(TipoViewModel model)
        {
            List<double> depreciaciones = new();

            double valorInicial = (double)model.CostoInicial;
            int vidaUtil = model.VidaUtilAnios;
            int numeroPeriodo = 1;

            while (numeroPeriodo <= vidaUtil && valorInicial > 0)
            {
                double depreciacion = valorInicial * (2.0 / vidaUtil);

                // Evitar valor negativo
                if (depreciacion > valorInicial)
                    depreciacion = valorInicial;

                depreciaciones.Add(depreciacion);

                valorInicial -= depreciacion;
                numeroPeriodo++;
            }

            return depreciaciones.ToArray();
        }


    }
}

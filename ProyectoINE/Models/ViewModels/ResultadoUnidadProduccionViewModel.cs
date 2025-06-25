using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProyectoINE.ViewModels
{
    public class ResultadoUnidadProduccionViewModel
    {
        public List<Fila> Filas { get; set; } = new List<Fila>();

        public decimal TotalDepreciacion => Filas.Sum(f => f.Depreciacion);
        public decimal TotalDeducibleFiscal => Filas.Sum(f => f.DeducibleFiscal);
        public decimal ValorFinalActivo => Filas.LastOrDefault()?.ValorFinal ?? 0;
        public int TotalUnidadesProducidas => Filas.Sum(f => f.UnidadesProducidas);

        public decimal DepreciacionPorUnidad { get; set; }
        public string? MetodoUtilizado { get; set; } = "Unidades de Producción";
        public DateTime FechaCalculo { get; set; } = DateTime.Now;

        public class Fila
        {
            [Display(Name = "Período")]
            public int NumeroPeriodo { get; set; }

            [Display(Name = "Valor Inicial")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal ValorInicial { get; set; }

            [Display(Name = "Unidades Producidas")]
            public int UnidadesProducidas { get; set; }

            [Display(Name = "Depreciación")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal Depreciacion { get; set; }

            [Display(Name = "Deducible Fiscal")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal DeducibleFiscal { get; set; }

            [Display(Name = "Valor Final")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal ValorFinal { get; set; }

            [Display(Name = "Depreciación por Unidad")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal DepreciacionPorUnidad { get; set; }

            [Display(Name = "% de Uso")]
            [DisplayFormat(DataFormatString = "{0:P2}")]
            public decimal PorcentajeUso { get; set; }

            public decimal CalcularDepreciacionAcumulada(decimal costoInicial) =>
                costoInicial == 0 ? 0 : ((costoInicial - ValorFinal) / costoInicial) * 100;
        }

        public decimal ObtenerDepreciacionPromedioPorPeriodo() =>
            Filas.Count > 0 ? TotalDepreciacion / Filas.Count : 0;

        public Fila? ObtenerPeriodoMayorDepreciacion() =>
            Filas.OrderByDescending(f => f.Depreciacion).FirstOrDefault();

        public Fila? ObtenerPeriodoMenorDepreciacion() =>
            Filas.Where(f => f.Depreciacion > 0).OrderBy(f => f.Depreciacion).FirstOrDefault();

        public bool ValidarCalculos(decimal costoInicial, decimal valorRescateEsperado)
        {
            var diferencia = Math.Abs(ValorFinalActivo - valorRescateEsperado);
            return diferencia < 0.01m;
        }
    }
}

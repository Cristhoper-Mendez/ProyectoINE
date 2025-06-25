using System.ComponentModel.DataAnnotations;

namespace ProyectoINE.ViewModels
{
    /// <summary>
    /// Modelo para representar una tabla de depreciación por el método de unidades de producción
    /// Específicamente diseñado para trabajar con unidades producidas
    /// </summary>
    public class TablaUnidadProduccion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Período")]
        public int Periodo { get; set; }

        [Required]
        [Display(Name = "Valor Inicial")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal ValorInicial { get; set; }

        [Display(Name = "Unidades Producidas")]
        public int UnidadesProducidas { get; set; }

        [Display(Name = "Unidades Acumuladas")]
        public int UnidadesAcumuladas { get; set; }

        [Display(Name = "Depreciación por Unidad")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        public decimal DepreciacionPorUnidad { get; set; }

        [Required]
        [Display(Name = "Depreciación del Período")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal DepreciacionPeriodo { get; set; }

        [Display(Name = "Depreciación Acumulada")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal DepreciacionAcumulada { get; set; }

        [Required]
        [Display(Name = "Valor Final")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal ValorFinal { get; set; }

        [Display(Name = "Deducible Fiscal")]
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal DeducibleFiscal { get; set; }

        [Display(Name = "Porcentaje de Uso")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public decimal PorcentajeUso { get; set; }

        [Display(Name = "Tasa de Depreciación")]
        [DisplayFormat(DataFormatString = "{0:P4}", ApplyFormatInEditMode = false)]
        public decimal TasaDepreciacion { get; set; }

        // Propiedades de auditoría
        [Display(Name = "Fecha de Cálculo")]
        public DateTime FechaCalculo { get; set; } = DateTime.Now;

        [Display(Name = "Método Utilizado")]
        [MaxLength(50)]
        public string MetodoDepreciacion { get; set; } = "Unidades de Producción";

        [Display(Name = "Observaciones")]
        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Propiedades calculadas (no se almacenan en BD)
        [Display(Name = "Eficiencia de Uso")]
        public decimal EficienciaUso
        {
            get
            {
                return UnidadesProducidas > 0 ? (DepreciacionPeriodo / UnidadesProducidas) : 0;
            }
        }

        [Display(Name = "Valor Depreciable Restante")]
        public decimal ValorDepreciableRestante
        {
            get
            {
                return ValorFinal > 0 ? ValorFinal : 0;
            }
        }

        // Métodos de utilidad
        public void CalcularValorFinal()
        {
            ValorFinal = ValorInicial - DepreciacionPeriodo;
            if (ValorFinal < 0) ValorFinal = 0;
        }

        public void CalcularPorcentajeUso(int totalUnidadesEstimadas)
        {
            if (totalUnidadesEstimadas > 0)
                PorcentajeUso = (decimal)UnidadesProducidas / totalUnidadesEstimadas;
        }

        public void CalcularTasaDepreciacion(decimal costoInicial)
        {
            if (costoInicial > 0)
                TasaDepreciacion = DepreciacionPeriodo / costoInicial;
        }

        public bool ValidarConsistencia()
        {
            // Validaciones básicas de consistencia
            return ValorInicial >= 0 &&
                   DepreciacionPeriodo >= 0 &&
                   ValorFinal >= 0 &&
                   UnidadesProducidas >= 0 &&
                   (ValorInicial - DepreciacionPeriodo) == ValorFinal;
        }

        public override string ToString()
        {
            return $"Período {Periodo}: {UnidadesProducidas} unidades, Depreciación: {DepreciacionPeriodo:C}";
        }
    }
}
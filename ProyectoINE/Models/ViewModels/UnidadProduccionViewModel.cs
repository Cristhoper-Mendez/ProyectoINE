using System.ComponentModel.DataAnnotations;
using ProyectoINE.ViewModels;

namespace ProyectoINE.ViewModels
{
    public class UnidadProduccionViewModel
    {
        [Required(ErrorMessage = "El costo inicial es requerido")]
        [Display(Name = "Costo Inicial ($)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo inicial debe ser mayor a 0")]
        public decimal CostoInicial { get; set; }

        [Required(ErrorMessage = "El valor de rescate es requerido")]
        [Display(Name = "Valor de Rescate ($)")]
        [Range(0, double.MaxValue, ErrorMessage = "El valor de rescate debe ser mayor o igual a 0")]
        public decimal ValorRescate { get; set; }

        [Required(ErrorMessage = "La vida útil es requerida")]
        [Display(Name = "Vida Útil (años)")]
        [Range(1, 100, ErrorMessage = "La vida útil debe estar entre 1 y 100 años")]
        public int VidaUtilAnios { get; set; }

        [Required(ErrorMessage = "Las unidades estimadas son requeridas")]
        [Display(Name = "Unidades Estimadas de Producción")]
        [Range(1, int.MaxValue, ErrorMessage = "Las unidades estimadas deben ser mayor a 0")]
        public int UnidadesEstimadas { get; set; }

        [Display(Name = "Impacto Fiscal (%)")]
        [Range(0, 100, ErrorMessage = "El impacto fiscal debe estar entre 0 y 100%")]
        public decimal ImpactoFiscal { get; set; } = 0;

        [Display(Name = "Descripción del Activo")]
        [MaxLength(200, ErrorMessage = "La descripción no puede exceder 200 caracteres")]
        public string? DescripcionActivo { get; set; }

        public bool EsValorRescateValido() => ValorRescate < CostoInicial;

        public decimal CalcularDepreciacionPorUnidad() =>
            UnidadesEstimadas == 0 ? 0 : (CostoInicial - ValorRescate) / UnidadesEstimadas;

        public decimal ObtenerValorDepreciable() => CostoInicial - ValorRescate;
    }
}

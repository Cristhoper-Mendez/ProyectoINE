using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProyectoINE.Models.ViewModels
{
    public class TipoViewModel
    {
        [Display(Name = "Costo inicial del activo")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Debe ingresar un valor mayor a 0")]
        public decimal CostoInicial { get; set; }

        [Display(Name = "Valor residual")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Debe ser mayor o igual a 0")]
        public decimal ValorResidual { get; set; }

        [Display(Name = "Vida útil en años")]
        [Range(1, 100, ErrorMessage = "Debe ingresar al menos 1 año")]
        public int VidaUtilAnios { get; set; }

        [Display(Name = "Tasa de uso del activo (%)")]
        [Range(1, 100, ErrorMessage = "Debe estar entre 1 y 100")]
        public int TasaUso { get; set; }

        [Display(Name = "Impacto fiscal (%)")]
        [Required]
        [Range(0, 100, ErrorMessage = "Debe estar entre 0 y 100")]
        public int ImpactoFiscal { get; set; }


        [Display(Name = "Tipo de depreciación")]
        [Required(ErrorMessage = "Debe seleccionar un tipo de activo")]
        public int? SelectedValue { get; set; }
        [Display(Name = "Tipos")]
        public List<SelectListItem> Items { get; set; }
    }
}

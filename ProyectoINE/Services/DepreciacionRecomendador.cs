using ProyectoINE.Models.Dtos;

namespace ProyectoINE.Services;

public class DepreciacionRecomendador
{
     public string RecomendarMetodo(
        double? costoInicial = null,
        double? valorResidual = null,
        int? vidaUtilAnios = null,
        double? tasaUso = null)
    {
        // **1. Validación de datos mínimos requeridos**
        if (!costoInicial.HasValue || costoInicial <= 0 || !vidaUtilAnios.HasValue || vidaUtilAnios <= 0)
        {
            return MetodoDepreciacion.NoAplicable;
        }

        // **2. Caso: Valor residual no proporcionado o mayor/igual que costo inicial**
        if (!valorResidual.HasValue || valorResidual >= costoInicial)
        {
            // Si no hay valor residual, se asume cero
            valorResidual = 0;
        }

      
        // **4. Evaluación de métodos basados en vida útil y depreciación acelerada**
        bool esVidaUtilCorta = vidaUtilAnios <= 5;
        bool esActivoDeAltoDesgaste = (costoInicial - valorResidual) / vidaUtilAnios > costoInicial * 0.25;

        if (esVidaUtilCorta && esActivoDeAltoDesgaste)
        {
            return (vidaUtilAnios <= 3) 
                ? MetodoDepreciacion.SaldoDecreciente 
                : MetodoDepreciacion.DigitosDeLosAnios;
        }

     

        // **6. Default: Línea Recta (para activos con desgaste uniforme)**
        return MetodoDepreciacion.LineaRecta;
    }
}
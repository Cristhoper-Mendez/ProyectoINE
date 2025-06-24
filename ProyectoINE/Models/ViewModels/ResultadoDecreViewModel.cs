namespace ProyectoINE.Models.ViewModels
{
    public class ResultadoDecreViewModel
    {
        public string Metodo => "Saldo decreciente";

        public List<Fila> Filas { get; set; } = new();

        public class Fila
        {
            public int NumeroPeriodo { get; set; }

            public decimal ValorInicial { get; set; }
            public decimal DeducibleFiscal { get; set; }

            public decimal Depreciacion { get; set; }

            public decimal ValorFinal { get; set; }
        }
    }
}

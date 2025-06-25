namespace ProyectoINE.Models.ViewModels
{
    public class SYDResultado
    {
        public double Depreciacion { get; set; }
        public double ValorLibro { get; set; }
        public double DepreciacionAcumulada { get; set; }
        public DatosSYD Datos { get; set; }
        public string? Error { get; set; }  // Esto sirve para controlar errores desde el controlador
    }
}

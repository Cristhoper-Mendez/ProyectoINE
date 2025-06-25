using ProyectoINE.Models;
using ProyectoINE.Models.ViewModels;

namespace ProyectoINE.Services
{
    public static class SYDService
    {
        public static SYDResultado Calcular(DatosSYD datos)
        {
            if (datos.N <= 0 || datos.k <= 0 || datos.k > datos.N)
            {
                return new SYDResultado
                {
                    Error = "Verifique que N y k sean válidos.",
                    Datos = datos
                };
            }

            double B = datos.B;
            double VR = datos.VR;
            int N = datos.N;
            int k = datos.k;

            double d_k = (B - VR) * (2.0 * (N - k + 1)) / (N * (N + 1));
            double parte1 = (2.0 * (B - VR) / N) * k;
            double parte2 = ((B - VR) * k * (k + 1)) / (N * (N + 1));
            double VL_k = B - parte1 + parte2;
            double dAcumulada = B - VL_k;

            return new SYDResultado
            {
                Depreciacion = d_k,
                ValorLibro = VL_k,
                DepreciacionAcumulada = dAcumulada,
                Datos = datos
            };
        }
    }
}

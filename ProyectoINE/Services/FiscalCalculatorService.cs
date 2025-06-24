using ProyectoINE.Models;

namespace ProyectoINE.Services
{
    public class FiscalCalculatorService
    {
        public string GetFiscalMethod(decimal[] metodo1, decimal[] metodo2, decimal[] metodo3, decimal[] metodo4)
        {
            if (metodo1 is null || metodo2 is null || metodo3 is null || metodo4 is null)
            {
                throw new ArgumentNullException("Ningún array puede ser nulo.");
            }

            if (metodo1 is null || metodo2 is null || metodo3 is null || metodo4 is null)
            {
                throw new ArgumentException("Ningún array puede estar vacío.");
            }

            decimal total1 = GetTotal(metodo1, 3);
            decimal total2 = GetTotal(metodo2, 3);
            decimal total3 = GetTotal(metodo3, 3);
            decimal total4 = GetTotal(metodo4, 3);

            string mensaje = "";

            decimal fiscal = Math.Max(Math.Max(total1, total2), Math.Max(total3, total4));
            switch (fiscal)
            {
                case var _ when fiscal == total1:
                    mensaje = $"Método Depreciación por Línea Recta tiene la mayor eficiencia: {total1:N2}";
                    break;
                case var _ when fiscal == total2:
                    mensaje = $"Método Depreciación por Saldo Decreciente tiene la mayor eficiencia: {total2:N2}";
                    break;
                case var _ when fiscal == total3:
                    mensaje = $"Método Suma de los Dígitos de los Años (SYD) tiene la mayor eficiencia: {total3:N2}";
                    break;
                case var _ when fiscal == total4:
                    mensaje = $"Método Unidad de Producción tiene la mayor eficiencia: {total4:N2}";
                    break;
                default:
                    mensaje = "Error inesperado al comparar desviaciones.";
                    break;
            }

            return mensaje;
        }

        public decimal GetTotal(decimal[] values, int k)
        {
            decimal result = 0;

            k = values.Length < k ? values.Length : k;

            for (int i = 0; i < k; i++)
            {
                result += values[i];
            }

            return result;
        }
    }
}

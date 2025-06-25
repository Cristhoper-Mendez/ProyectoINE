using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoINE.Models;
using ProyectoINE.Models.ViewModels;
using System.Diagnostics;
using MathNet.Numerics.Statistics;
using ProyectoINE.Services;

namespace ProyectoINE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        
        [HttpGet]
        public IActionResult Index()
        {
            // Creamos el modelo para enviar a la vista
            var model = new TipoViewModel
            {
                Items = ObtenerTiposDeDepr()
            };

            return View(model);
        }

        // Método para manejar el envío del formulario desde la vista
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calcular(TipoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Debug.WriteLine($"Error en '{entry.Key}': {error.ErrorMessage}");
                    }
                }
                // Si el modelo no es válido, volvemos a mostrar la vista con los errores
                model.Items = ObtenerTiposDeDepr();
                return View("Index", model);
            }

            object modeloResultado;
            string vistaDestino;
            var recomendador = new DepreciacionRecomendador();

            // Redirigimos al método elegido
            switch (model.SelectedValue)
            {
                case 1:
                    var metodo1 = recomendador.RecomendarMetodo((double?)model.CostoInicial,(double?)model.ValorResidual,model.VidaUtilAnios,model.TasaUso);
                    Console.WriteLine($"Caso Recomendado: {metodo1.ToString()}");
                    ViewBag.AlertMessage = metodo1.ToString();
                    model.Items = ObtenerTiposDeDepr();
                    return View("Index",model);
                      break;
                case 2:
                    // Método de línea recta
                    var lrecta = new LineaRectaController();
                    modeloResultado = lrecta.Calcular(model);
                    vistaDestino = "LineaRectaTable";
                    break;
                case 3:
                    // Método de saldos decrecientes
                    var decreciente = new DecreController();
                    modeloResultado = decreciente.Calcular(model);
                    vistaDestino = "SaldoDecrecienteTable";
                    break;
                case 4:
                    // Método de suma de los dígitos de los años
                    var datosSYD = new DatosSYD
                    {
                        B = (double)model.CostoInicial,
                        VR = (double)model.ValorResidual,
                        N = model.VidaUtilAnios,
                        k = model.TasaUso
                    };

                    var resultado = SYDService.Calcular(datosSYD);

                    if (!string.IsNullOrEmpty(resultado.Error))
                    {
                        ModelState.AddModelError("", resultado.Error);
                        model.Items = ObtenerTiposDeDepr();
                        return View("Index", model);
                    }
                    vistaDestino = "SYDTable";
                    modeloResultado = resultado;
                    break;
                /*case 5:
                    // Método de unidades de producción
                    break;*/
                case 6:
                    // Método de evaluación fiscal
                    // Array método 1
                    var lctrl = new LineaRectaController();
                    decimal[] larray = lctrl.CalcularDepreciaciones(model)
                        .Select(d => (decimal)d)
                        .ToArray();
                    // Array método 2
                    var decrectrl = new DecreController();
                    decimal[] decre_array = decrectrl.CalcularDepreciaciones(model)
                        .Select(d => (decimal)d)
                        .ToArray();
                    // Array método 3
                    var datosSYDservice = new DatosSYD
                    {
                        B = (double)model.CostoInicial,
                        VR = (double)model.ValorResidual,
                        N = model.VidaUtilAnios,
                        k = model.TasaUso
                    };
                    decimal[] syd_array = SYDService.CalcularDepreciacionesSYD(datosSYDservice)
                        .Select(d => (decimal)d)
                        .ToArray();
                    // Array método 4


                    var evfiscal = new FiscalCalculatorService();
                    modeloResultado = evfiscal.GetFiscalMethod(larray, decre_array, syd_array, );
                    ViewBag.MensajeFiscal = modeloResultado.ToString();
                    vistaDestino = "Index";
                    break;
                case 7:
                    // Método de evaluación contable
                    // Array método 1
                    var lctrl2 = new LineaRectaController();
                    double[] larray2 = lctrl2.CalcularDepreciaciones(model);
                    // Array método 2
                    var decrectrl2 = new DecreController();
                    double[] decre_array2 = decrectrl2.CalcularDepreciaciones(model);
                    // Array método 3
                    var datosSYDservice2 = new DatosSYD
                    {
                        B = (double)model.CostoInicial,
                        VR = (double)model.ValorResidual,
                        N = model.VidaUtilAnios,
                        k = model.TasaUso
                    };
                    double[] syd_array2 = SYDService.CalcularDepreciacionesSYD(datosSYDservice2);
                    // Array método 4


                    var evCntbl = new FiscalCalculatorService();
                    modeloResultado = this.CalcularDesviacionEstandarMasBaja(larray2, decre_array2, syd_array2,);
                    ViewBag.MensajeContable = modeloResultado.ToString();
                    vistaDestino = "Index";
                    break;

                default:
                    model.Items = ObtenerTiposDeDepr();
                    return View("Index", model);
            }

            return View(vistaDestino, modeloResultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string CalcularDesviacionEstandarMasBaja(
            double[] Metodo1, 
            double[] Metodo2,
            double[] Metodo3, 
            double[] Metodo4)
        {
            // Validar que los arrays no sean nulos o vacíos
            if (Metodo1 == null || Metodo2 == null || Metodo3 == null || Metodo4 == null)
                throw new ArgumentNullException("Ningún array puede ser nulo.");
    
            if (Metodo1.Length == 0 || Metodo2.Length == 0 || Metodo3.Length == 0 || Metodo4.Length == 0)
                throw new ArgumentException("Ningún array puede estar vacío.");

            // Calcular desviaciones estándar
            double sd1 = Metodo1.StandardDeviation();
            double sd2 = Metodo2.StandardDeviation();
            double sd3 = Metodo3.StandardDeviation();
            double sd4 = Metodo4.StandardDeviation();

            // Determinar cuál es la menor
            double minSD = Math.Min(Math.Min(sd1, sd2), Math.Min(sd3, sd4));

            // Usar switch-case para identificar el método ganador
            string mensaje;
            switch (minSD)
            {
                case var _ when minSD == sd1:
                    mensaje = $"Método Depreciación por Línea Recta tiene la menor desviación estándar: {sd1:N2}";
                    break;
                case var _ when minSD == sd2:
                    mensaje = $"Método Depreciación por Saldo Decreciente tiene la menor desviación estándar: {sd2:N2}";
                    break;
                case var _ when minSD == sd3:
                    mensaje = $"Método Suma de los Dígitos de los Años (SYD) tiene la menor desviación estándar: {sd3:N2}";
                    break;
                case var _ when minSD == sd4:
                    mensaje = $"Método Unidad de Producción tiene la menor desviación estándar: {sd4:N2}";
                    break;
                default:
                    mensaje = "Error inesperado al comparar desviaciones.";
                    break;
            }

            return mensaje;
        }
     

        // M�todo para obtener los tipos de depreciaci�n
        private List<SelectListItem> ObtenerTiposDeDepr()
        {
            return new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Evaluar metodo optimo" },
            new SelectListItem { Value = "2", Text = "Linea Recta" },
            new SelectListItem { Value = "3", Text = "Saldo Decreciente" },
            new SelectListItem { Value = "4", Text = "Digitos de los anios" },
            new SelectListItem { Value = "5", Text = "Unidades de produccion" },
            new SelectListItem { Value = "6", Text = "Evaluacion Fiscal" },
            new SelectListItem { Value = "7", Text = "Evaluacion contable" }
        };
        }
    }
}

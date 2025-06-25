using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Configurar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configuración de rutas específica para UnidadProduccionController
app.MapControllerRoute(
    name: "unidadProduccion",
    pattern: "UnidadProduccion/{action=Index}/{id?}",
    defaults: new { controller = "UnidadProduccion" });

// Ruta por defecto (redirige a UnidadProduccion/Index)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UnidadProduccion}/{action=Index}/{id?}");

app.Run();

using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class InformesController : Controller
{
    private readonly IRepositorioInmueble repositorioInmueble;

    public InformesController(IRepositorioInmueble repositorioInmueble)
    {
        this.repositorioInmueble = repositorioInmueble;
    }

    // Menú principal de informes
    public IActionResult Index()
    {
        return View();
    }

    // Informe 1: Listar inmuebles y su dueño, con filtro opcional por disponibilidad
    
    public IActionResult InmueblesPorDisponibilidad(bool? disponible, int pagina = 1)
    {
        var tamPagina = 10;
        var lista = repositorioInmueble.ObtenerPorDisponibilidad(disponible, pagina, tamPagina);
        var cantidad = repositorioInmueble.ObtenerCantidadPorDisponibilidad(disponible);

        ViewBag.Disponible = disponible;
        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling(cantidad / (double)tamPagina);

        return View(lista);
    }
}
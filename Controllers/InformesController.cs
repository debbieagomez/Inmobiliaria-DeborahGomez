using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inmobiliaria_DeborahGomez.Controllers;


[Authorize] public class InformesController : Controller
{
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioPropietario repositorioPropietario;

    public InformesController(IRepositorioInmueble repositorioInmueble, IRepositorioPropietario repositorioPropietario)
    {
        this.repositorioInmueble = repositorioInmueble;
        this.repositorioPropietario = repositorioPropietario;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Informe 1
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

    // Informe 2
    public IActionResult InmueblesPorPropietario(int? propietarioId, int pagina = 1)
    {
        var tamPagina = 10;

        ViewBag.Propietarios = repositorioPropietario.ObtenerLista(null, 1, 1000);
        ViewBag.PropietarioIdSeleccionado = propietarioId;
        ViewBag.Pagina = pagina;

        if (propietarioId == null)
        {
            ViewBag.TotalPaginas = 0;
            return View(new List<Inmobiliaria_DeborahGomez.Models.Inmueble>());
        }

        var lista = repositorioInmueble.ObtenerPorPropietario(propietarioId.Value, pagina, tamPagina);
        var cantidad = repositorioInmueble.ObtenerCantidadPorPropietario(propietarioId.Value);

        ViewBag.TotalPaginas = (int)Math.Ceiling(cantidad / (double)tamPagina);

        return View(lista);
    }

    // Informe 3
    public IActionResult InmueblesMasReservados(int dias = 365, int pagina = 1)
    {
        var tamPagina = 10;

        var lista = repositorioInmueble.ObtenerMasReservados(dias, pagina, tamPagina);
        var cantidad = repositorioInmueble.ObtenerCantidadMasReservados(dias);

        ViewBag.Dias = dias;
        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling(cantidad / (double)tamPagina);

        return View(lista);
    }

    // Informe 4
    public IActionResult InmueblesSinReservas(int dias = 30, int pagina = 1)
    {
        var tamPagina = 10;

        var lista = repositorioInmueble.ObtenerSinReservas(dias, pagina, tamPagina);
        var cantidad = repositorioInmueble.ObtenerCantidadSinReservas(dias);

        ViewBag.Dias = dias;
        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = (int)Math.Ceiling(cantidad / (double)tamPagina);

        return View(lista);
    }

    // Informe 5: Inmuebles disponibles (no ocupados) entre dos fechas
    public IActionResult InmueblesDisponiblesEntreFechas(DateTime? fechaDesde, DateTime? fechaHasta, int pagina = 1)
    {
        var tamPagina = 10;

        ViewBag.FechaDesde = fechaDesde;
        ViewBag.FechaHasta = fechaHasta;
        ViewBag.Pagina = pagina;

        if (fechaDesde == null || fechaHasta == null)
        {
            ViewBag.TotalPaginas = 0;
            return View(new List<Inmobiliaria_DeborahGomez.Models.Inmueble>());
        }

        if (fechaDesde >= fechaHasta)
        {
            ModelState.AddModelError("", "La fecha desde debe ser anterior a la fecha hasta.");
            ViewBag.TotalPaginas = 0;
            return View(new List<Inmobiliaria_DeborahGomez.Models.Inmueble>());
        }

        var lista = repositorioInmueble.ObtenerDisponiblesEntreFechas(fechaDesde.Value, fechaHasta.Value, pagina, tamPagina);
        var cantidad = repositorioInmueble.ObtenerCantidadDisponiblesEntreFechas(fechaDesde.Value, fechaHasta.Value);

        ViewBag.TotalPaginas = (int)Math.Ceiling(cantidad / (double)tamPagina);

        return View(lista);
    }
}
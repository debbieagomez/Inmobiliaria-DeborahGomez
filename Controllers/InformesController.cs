using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

[Authorize]
public class InformesController : Controller
{
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioPropietario repositorioPropietario;
    private readonly IRepositorioReserva repositorioReserva;

    public InformesController(
        IRepositorioInmueble repositorioInmueble,
        IRepositorioPropietario repositorioPropietario, 
        IRepositorioReserva repositorioReserva)
    {
        this.repositorioInmueble =
            repositorioInmueble;

        this.repositorioPropietario =
            repositorioPropietario;

        this.repositorioReserva = repositorioReserva;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult InmueblesPorDisponibilidad(
        bool? disponible,
        int pagina = 1)
    {
        pagina =
            Math.Max(
                pagina,
                1
            );

        const int tamPagina = 10;

        var lista =
            repositorioInmueble
                .ObtenerPorDisponibilidad(
                    disponible,
                    pagina,
                    tamPagina
                );

        var cantidad =
            repositorioInmueble
                .ObtenerCantidadPorDisponibilidad(
                    disponible
                );

        ViewBag.Disponible =
            disponible;

        ViewBag.Pagina =
            pagina;

        ViewBag.TotalPaginas =
            (int)Math.Ceiling(
                cantidad /
                (double)tamPagina
            );

        return View(lista);
    }


    public IActionResult InmueblesPorPropietario(
        int? propietarioId,
        int pagina = 1)
    {
        pagina =
            Math.Max(
                pagina,
                1
            );

        const int tamPagina = 10;

        ViewBag.Propietarios =
            repositorioPropietario
                .ObtenerLista(
                    null,
                    1,
                    1000
                );

        ViewBag.PropietarioIdSeleccionado =
            propietarioId;

        ViewBag.Pagina =
            pagina;

        if (propietarioId == null)
        {
            ViewBag.TotalPaginas = 0;

            return View(
                new List<Inmueble>()
            );
        }

        var lista =
            repositorioInmueble
                .ObtenerPorPropietario(
                    propietarioId.Value,
                    pagina,
                    tamPagina
                );

        var cantidad =
            repositorioInmueble
                .ObtenerCantidadPorPropietario(
                    propietarioId.Value
                );

        ViewBag.TotalPaginas =
            (int)Math.Ceiling(
                cantidad /
                (double)tamPagina
            );

        return View(lista);
    }

 
    public IActionResult InmueblesMasReservados(
        int dias = 365,
        int pagina = 1)
    {
        pagina =
            Math.Max(
                pagina,
                1
            );

        if (dias <= 0)
        {
            dias = 365;
        }

        const int tamPagina = 10;

        var lista =
            repositorioInmueble
                .ObtenerMasReservados(
                    dias,
                    pagina,
                    tamPagina
                );

        var cantidad =
            repositorioInmueble
                .ObtenerCantidadMasReservados(
                    dias
                );

        ViewBag.Dias =
            dias;

        ViewBag.Pagina =
            pagina;

        ViewBag.TotalPaginas =
            (int)Math.Ceiling(
                cantidad /
                (double)tamPagina
            );

        return View(lista);
    }


    public IActionResult InmueblesSinReservas(
        int dias = 30,
        int pagina = 1)
    {
        pagina =
            Math.Max(
                pagina,
                1
            );

        if (dias <= 0)
        {
            dias = 30;
        }

        const int tamPagina = 10;

        var lista =
            repositorioInmueble
                .ObtenerSinReservas(
                    dias,
                    pagina,
                    tamPagina
                );

        var cantidad =
            repositorioInmueble
                .ObtenerCantidadSinReservas(
                    dias
                );

        ViewBag.Dias =
            dias;

        ViewBag.Pagina =
            pagina;

        ViewBag.TotalPaginas =
            (int)Math.Ceiling(
                cantidad /
                (double)tamPagina
            );

        return View(lista);
    }

     public IActionResult InmueblesDisponiblesEntreFechas(
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int pagina = 1)
    {
        pagina =
            Math.Max(
                pagina,
                1
            );

        const int tamPagina = 10;

        ViewBag.FechaDesde =
            fechaDesde;

        ViewBag.FechaHasta =
            fechaHasta;

        ViewBag.Pagina =
            pagina;

        if (
            fechaDesde == null ||
            fechaHasta == null
        )
        {
            ViewBag.TotalPaginas = 0;

            return View(
                new List<Inmueble>()
            );
        }

        if (fechaDesde >= fechaHasta)
        {
            ModelState.AddModelError(
                "",
                "La fecha desde debe ser anterior a la fecha hasta."
            );

            ViewBag.TotalPaginas = 0;

            return View(
                new List<Inmueble>()
            );
        }

        var lista =
            repositorioInmueble
                .ObtenerDisponiblesEntreFechas(
                    fechaDesde.Value,
                    fechaHasta.Value,
                    pagina,
                    tamPagina
                );

        var cantidad =
            repositorioInmueble
                .ObtenerCantidadDisponiblesEntreFechas(
                    fechaDesde.Value,
                    fechaHasta.Value
                );

        ViewBag.TotalPaginas =
            (int)Math.Ceiling(
                cantidad /
                (double)tamPagina
            );

        return View(lista);
    }

    public IActionResult ReservasVigentes(
    int pagina = 1)
    {
        pagina = Math.Max(
            pagina,
            1
        );

        const int tamPagina = 10;

        var cantidad =
            repositorioReserva.ObtenerCantidadVigentes();

        var totalPaginas =
            cantidad == 0
                ? 1
                : (int)Math.Ceiling(
                    cantidad / (double)tamPagina
                );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var lista =
            repositorioReserva.ObtenerVigentes(
                pagina,
                tamPagina
            );

        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = totalPaginas;

        return View(lista);
    }

    public IActionResult ReservasQueTerminan(
        int dias = 7,
        int pagina = 1)
    {
        pagina = Math.Max(
            pagina,
            1
        );

        if (dias <= 0)
        {
            dias = 7;
        }

        const int tamPagina = 10;

        var cantidad =
            repositorioReserva.ObtenerCantidadQueTerminanEn(
                dias
            );

        var totalPaginas =
            cantidad == 0
                ? 1
                : (int)Math.Ceiling(
                    cantidad / (double)tamPagina
                );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var lista =
            repositorioReserva.ObtenerQueTerminanEn(
                dias,
                pagina,
                tamPagina
            );

        ViewBag.Dias = dias;
        ViewBag.Pagina = pagina;
        ViewBag.TotalPaginas = totalPaginas;

        return View(lista);
    }
}
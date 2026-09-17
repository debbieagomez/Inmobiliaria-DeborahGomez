using System.Security.Claims;
using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class ReservasController : ABMController<Reserva>
{
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioInquilino repositorioInquilino;
    private readonly IRepositorioTipoInmueble repositorioTipoInmueble;

    public ReservasController(
        IRepositorioReserva repositorio,
        IRepositorioInmueble repositorioInmueble,
        IRepositorioInquilino repositorioInquilino,
        IRepositorioTipoInmueble repositorioTipoInmueble)
        : base(repositorio)
    {
        this.repositorioInmueble = repositorioInmueble;
        this.repositorioInquilino = repositorioInquilino;
        this.repositorioTipoInmueble = repositorioTipoInmueble;
    }

    public override void OnActionExecuting(
        ActionExecutingContext context)
    {
        ViewBag.Inmuebles =
            repositorioInmueble.ObtenerLista(
                tamPagina: 1000
            );

        ViewBag.Inquilinos =
            repositorioInquilino.ObtenerLista(
                tamPagina: 1000
            );

        ViewBag.TiposInmueble =
            repositorioTipoInmueble.ObtenerLista(
                tamPagina: 1000
            );

        base.OnActionExecuting(context);
    }

    [HttpGet]
    public override IActionResult Crear()
    {
        return View(new Reserva());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override IActionResult Crear(
        Reserva reserva)
    {
        var repositorioReserva =
            (IRepositorioReserva)repositorio;

        var claimUsuarioId =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

        if (!int.TryParse(
            claimUsuarioId,
            out var usuarioId))
        {
            return Unauthorized();
        }

        reserva.UsuarioCreadorId =
            usuarioId;

        reserva.FechaHastaOriginal =
            reserva.FechaHasta;

        reserva.Finalizada = false;

        reserva.FechaFinalizacionAnticipada =
            null;

        reserva.MontoMulta =
            null;

        reserva.UsuarioFinalizadorId =
            null;

        if (reserva.FechaHasta < reserva.FechaDesde)
        {
            ModelState.AddModelError(
                "FechaHasta",
                "La fecha de finalización no puede ser anterior a la fecha de inicio."
            );
        }

        var inmueble =
            repositorioInmueble.ObtenerPorId(
                reserva.InmuebleId
            );

        if (inmueble == null)
        {
            ModelState.AddModelError(
                "InmuebleId",
                "El inmueble seleccionado no existe."
            );
        }

        if (inmueble != null)
        {
            if (inmueble.PorcentajeSenia < 0 ||
                inmueble.PorcentajeSenia > 100)
            {
                ModelState.AddModelError(
                    "InmuebleId",
                    "El porcentaje de seña del inmueble debe estar entre 0 y 100."
                );
            }

            reserva.MontoPorDia =
                inmueble.PrecioPorDia;
        }

        if (inmueble != null &&
            ModelState.IsValid)
        {
            if (repositorioReserva.ExisteSolapamiento(
                reserva.InmuebleId,
                reserva.FechaDesde,
                reserva.FechaHasta))
            {
                ModelState.AddModelError(
                    "FechaHasta",
                    "El inmueble ya está reservado en esas fechas."
                );
            }
        }

        if (!ModelState.IsValid)
        {
            return View(reserva);
        }

        try
        {
            repositorioReserva.AltaConSena(
                reserva,
                inmueble!.PorcentajeSenia
            );
        }
        catch
        {
            ModelState.AddModelError(
                "",
                "No se pudo crear la reserva y su pago de seña."
            );

            return View(reserva);
        }

        return RedirectToAction(
            nameof(Index)
        );
    }

    [HttpGet]
    public override IActionResult Editar(int id)
    {
        var reserva =
            repositorio.ObtenerPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override IActionResult Editar(
        Reserva reserva)
    {
        var repositorioReserva =
            (IRepositorioReserva)repositorio;

        var reservaActual =
            repositorioReserva.ObtenerPorId(
                reserva.IdReserva
            );

        if (reservaActual == null)
        {
            return NotFound();
        }

        reserva.FechaHastaOriginal =
            reservaActual.FechaHastaOriginal;

        reserva.UsuarioCreadorId =
            reservaActual.UsuarioCreadorId;

        reserva.UsuarioFinalizadorId =
            reservaActual.UsuarioFinalizadorId;

        reserva.Finalizada =
            reservaActual.Finalizada;

        reserva.FechaFinalizacionAnticipada =
            reservaActual.FechaFinalizacionAnticipada;

        reserva.MontoMulta =
            reservaActual.MontoMulta;

        if (repositorioReserva.ExisteSolapamiento(
            reserva.InmuebleId,
            reserva.FechaDesde,
            reserva.FechaHasta,
            reserva.IdReserva))
        {
            ModelState.AddModelError(
                "FechaHasta",
                "El inmueble ya está reservado en esas fechas."
            );
        }

        if (!ModelState.IsValid)
        {
            return View(reserva);
        }

        repositorioReserva.Modificacion(
            reserva
        );

        return RedirectToAction(
            nameof(Index)
        );
    }

    [HttpGet]
    public IActionResult BuscarDisponibles(
        DateTime? fechaDesde,
        DateTime? fechaHasta,
        int? cupo,
        int? tipoInmuebleId,
        decimal? precioMaximo)
    {
        var resultados =
            new List<Inmueble>();

        if (fechaDesde.HasValue &&
            fechaHasta.HasValue)
        {
            if (fechaHasta.Value < fechaDesde.Value)
            {
                ModelState.AddModelError(
                    "FechaHasta",
                    "La fecha de finalización no puede ser anterior a la fecha de inicio."
                );
            }
            else
            {
                var repositorioReserva =
                    (IRepositorioReserva)repositorio;

                resultados =
                    repositorioReserva.BuscarDisponibles(
                        fechaDesde.Value,
                        fechaHasta.Value,
                        cupo,
                        tipoInmuebleId,
                        precioMaximo
                    ).ToList();
            }
        }

        ViewBag.FechaDesde =
            fechaDesde;

        ViewBag.FechaHasta =
            fechaHasta;

        ViewBag.Cupo =
            cupo;

        ViewBag.TipoInmuebleId =
            tipoInmuebleId;

        ViewBag.PrecioMaximo =
            precioMaximo;

        return View(resultados);
    }
}
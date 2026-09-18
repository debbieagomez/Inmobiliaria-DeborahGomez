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
    private readonly IRepositorioPago repositorioPago;

    public ReservasController(
        IRepositorioReserva repositorio,
        IRepositorioInmueble repositorioInmueble,
        IRepositorioInquilino repositorioInquilino,
        IRepositorioTipoInmueble repositorioTipoInmueble,
        IRepositorioPago repositorioPago)
        : base(repositorio)
    {
        this.repositorioInmueble =
            repositorioInmueble;

        this.repositorioInquilino =
            repositorioInquilino;

        this.repositorioTipoInmueble =
            repositorioTipoInmueble;

        this.repositorioPago =
            repositorioPago;
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
        var hoy = DateTime.Now.Date;

        return View(new Reserva
        {
            FechaDesde = hoy,
            FechaHasta = hoy.AddDays(1),
            FechaHastaOriginal = hoy.AddDays(1),
            Finalizada = false,
            MontoMulta = null,
            FechaFinalizacionAnticipada = null,
            UsuarioFinalizadorId = null
        });
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

        if (
            reserva.FechaHasta <=
            reserva.FechaDesde
        )
        {
            ModelState.AddModelError(
                "FechaHasta",
                "La fecha de finalización debe ser posterior a la fecha de inicio."
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
            if (
                inmueble.PorcentajeSenia < 0 ||
                inmueble.PorcentajeSenia > 100
            )
            {
                ModelState.AddModelError(
                    "InmuebleId",
                    "El porcentaje de seña del inmueble debe estar entre 0 y 100."
                );
            }

            reserva.MontoPorDia =
                inmueble.PrecioPorDia;
        }

        if (
            inmueble != null &&
            ModelState.IsValid
        )
        {
            var existeSolapamiento =
                repositorioReserva.ExisteSolapamiento(
                    reserva.InmuebleId,
                    reserva.FechaDesde,
                    reserva.FechaHasta
                );

            if (existeSolapamiento)
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
    public override IActionResult Editar(
        int id)
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

        if (
            reserva.FechaHasta <=
            reserva.FechaDesde
        )
        {
            ModelState.AddModelError(
                "FechaHasta",
                "La fecha de finalización debe ser posterior a la fecha de inicio."
            );
        }

        if (
            ModelState.IsValid &&
            repositorioReserva.ExisteSolapamiento(
                reserva.InmuebleId,
                reserva.FechaDesde,
                reserva.FechaHasta,
                reserva.IdReserva
            )
        )
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

        if (
            fechaDesde.HasValue &&
            fechaHasta.HasValue
        )
        {
            if (
                fechaHasta.Value <=
                fechaDesde.Value
            )
            {
                ModelState.AddModelError(
                    "FechaHasta",
                    "La fecha de finalización debe ser posterior a la fecha de inicio."
                );
            }
            else
            {
                var repositorioReserva =
                    (IRepositorioReserva)repositorio;

                resultados =
                    repositorioReserva
                        .BuscarDisponibles(
                            fechaDesde.Value,
                            fechaHasta.Value,
                            cupo,
                            tipoInmuebleId,
                            precioMaximo
                        )
                        .ToList();
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

    [HttpGet]
    public IActionResult Detalle(
        int id)
    {
        var reserva =
            repositorio.ObtenerPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        return View(reserva);
    }

    [HttpGet]
    public IActionResult Renovar(
        int id)
    {
        var reservaOriginal =
            repositorio.ObtenerPorId(id);

        if (reservaOriginal == null)
        {
            return NotFound();
        }

        var inmueble =
            repositorioInmueble.ObtenerPorId(
                reservaOriginal.InmuebleId
            );

        if (inmueble == null)
        {
            return NotFound();
        }

        var inquilino =
            repositorioInquilino.ObtenerPorId(
                reservaOriginal.InquilinoId
            );

        if (inquilino == null)
        {
            return NotFound();
        }

        var fechaInicioNueva =
            reservaOriginal.FechaFinalizacionAnticipada
            ?? reservaOriginal.FechaHasta;

        var nuevaReserva =
            new Reserva
            {
                IdReserva =
                    reservaOriginal.IdReserva,

                InmuebleId =
                    reservaOriginal.InmuebleId,

                InquilinoId =
                    reservaOriginal.InquilinoId,

                FechaDesde =
                    fechaInicioNueva,

                FechaHasta =
                    fechaInicioNueva.AddDays(1),

                FechaHastaOriginal =
                    fechaInicioNueva.AddDays(1),

                MontoPorDia =
                    inmueble.PrecioPorDia,

                Finalizada =
                    false,

                FechaFinalizacionAnticipada =
                    null,

                MontoMulta =
                    null,

                UsuarioCreadorId =
                    0,

                UsuarioFinalizadorId =
                    null
            };

        ViewBag.Inmueble =
            inmueble;

        ViewBag.Inquilino =
            inquilino;

        ViewBag.FechaInicioMinima =
            fechaInicioNueva;

        return View(
            nuevaReserva
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Renovar(
        Reserva reserva)
    {
        var reservaOriginal =
            repositorio.ObtenerPorId(
                reserva.IdReserva
            );

        if (reservaOriginal == null)
        {
            return NotFound();
        }

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

        var inmueble =
            repositorioInmueble.ObtenerPorId(
                reservaOriginal.InmuebleId
            );

        if (inmueble == null)
        {
            return NotFound();
        }

        var inquilino =
            repositorioInquilino.ObtenerPorId(
                reservaOriginal.InquilinoId
            );

        if (inquilino == null)
        {
            return NotFound();
        }

        var fechaInicioMinima =
            reservaOriginal.FechaFinalizacionAnticipada
            ?? reservaOriginal.FechaHasta;

        reserva.InmuebleId =
            reservaOriginal.InmuebleId;

        reserva.InquilinoId =
            reservaOriginal.InquilinoId;

        if (
            reserva.FechaDesde <
            fechaInicioMinima
        )
        {
            ModelState.AddModelError(
                "FechaDesde",
                $"La nueva reserva debe comenzar el {fechaInicioMinima:dd/MM/yyyy} o después."
            );
        }

        if (
            reserva.FechaHasta <=
            reserva.FechaDesde
        )
        {
            ModelState.AddModelError(
                "FechaHasta",
                "La fecha de finalización debe ser posterior a la fecha de inicio."
            );
        }

        if (
            reserva.MontoPorDia <= 0
        )
        {
            ModelState.AddModelError(
                "MontoPorDia",
                "El monto por día debe ser mayor a cero."
            );
        }

        var repositorioReserva =
            (IRepositorioReserva)repositorio;

        if (
            ModelState.IsValid &&
            repositorioReserva.ExisteSolapamiento(
                reserva.InmuebleId,
                reserva.FechaDesde,
                reserva.FechaHasta
            )
        )
        {
            ModelState.AddModelError(
                "FechaHasta",
                "El inmueble ya está reservado en esas fechas."
            );
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Inmueble =
                inmueble;

            ViewBag.Inquilino =
                inquilino;

            ViewBag.FechaInicioMinima =
                fechaInicioMinima;

            return View(
                reserva
            );
        }

        var nuevaReserva =
            new Reserva
            {
                FechaDesde =
                    reserva.FechaDesde,

                FechaHasta =
                    reserva.FechaHasta,

                FechaHastaOriginal =
                    reserva.FechaHasta,

                MontoPorDia =
                    reserva.MontoPorDia,

                Finalizada =
                    false,

                FechaFinalizacionAnticipada =
                    null,

                MontoMulta =
                    null,

                InmuebleId =
                    reservaOriginal.InmuebleId,

                InquilinoId =
                    reservaOriginal.InquilinoId,

                UsuarioCreadorId =
                    usuarioId,

                UsuarioFinalizadorId =
                    null
            };

        try
        {
            repositorioReserva.AltaConSena(
                nuevaReserva,
                inmueble.PorcentajeSenia
            );
        }
        catch
        {
            ModelState.AddModelError(
                "",
                "No se pudo crear la nueva reserva."
            );

            ViewBag.Inmueble =
                inmueble;

            ViewBag.Inquilino =
                inquilino;

            ViewBag.FechaInicioMinima =
                fechaInicioMinima;

            return View(
                reserva
            );
        }

        return RedirectToAction(
            nameof(Detalle),
            new
            {
                id =
                    nuevaReserva.IdReserva
            }
        );
    }

    [HttpGet]
    public IActionResult Finalizar(
        int id,
        DateTime? fechaFinalizacion)
    {
        var reserva =
            repositorio.ObtenerPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        if (reserva.Finalizada)
        {
            return BadRequest(
                "La reserva ya fue finalizada."
            );
        }

        ViewBag.MontoMultaCalculada =
            0m;

        ViewBag.PorcentajeMulta =
            0m;

        ViewBag.MultaAbonada =
            false;

        ViewBag.FechaFinalizacion =
            fechaFinalizacion;

        if (fechaFinalizacion.HasValue)
        {
            if (
                fechaFinalizacion.Value <=
                reserva.FechaDesde ||
                fechaFinalizacion.Value >=
                reserva.FechaHastaOriginal
            )
            {
                ModelState.AddModelError(
                    "fechaFinalizacion",
                    "La fecha de finalización no es válida."
                );
            }
            else
            {
                var datos =
                    CalcularMulta(
                        reserva,
                        fechaFinalizacion.Value
                    );

                ViewBag.MontoMultaCalculada =
                    datos.MontoMulta;

                ViewBag.PorcentajeMulta =
                    datos.PorcentajeMulta;

                ViewBag.MultaAbonada =
                    repositorioPago.ExistePagoMulta(
                        id,
                        datos.MontoMulta
                    );
            }
        }

        return View(reserva);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Finalizar(
        int id,
        DateTime fechaFinalizacion)
    {
        var reserva =
            repositorio.ObtenerPorId(id);

        if (reserva == null)
        {
            return NotFound();
        }

        if (reserva.Finalizada)
        {
            return BadRequest(
                "La reserva ya fue finalizada."
            );
        }

        if (
            fechaFinalizacion <=
            reserva.FechaDesde ||
            fechaFinalizacion >=
            reserva.FechaHastaOriginal
        )
        {
            ModelState.AddModelError(
                "fechaFinalizacion",
                "La fecha de finalización debe ser posterior al inicio y anterior a la fecha original."
            );

            return View(reserva);
        }

        var datos =
            CalcularMulta(
                reserva,
                fechaFinalizacion
            );

        ViewBag.MontoMultaCalculada =
            datos.MontoMulta;

        ViewBag.PorcentajeMulta =
            datos.PorcentajeMulta;

        ViewBag.FechaFinalizacion =
            fechaFinalizacion;

        var multaAbonada =
            repositorioPago.ExistePagoMulta(
                id,
                datos.MontoMulta
            );

        ViewBag.MultaAbonada =
            multaAbonada;

        if (!multaAbonada)
        {
            ModelState.AddModelError(
                "",
                "La multa debe estar abonada antes de finalizar la reserva."
            );

            return View(reserva);
        }

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

        try
        {
            ((IRepositorioReserva)repositorio)
                .FinalizarAnticipadamente(
                    id,
                    fechaFinalizacion,
                    usuarioId
                );
        }
        catch
        {
            ModelState.AddModelError(
                "",
                "No se pudo finalizar anticipadamente la reserva."
            );

            return View(reserva);
        }

        return RedirectToAction(
            nameof(Detalle),
            new
            {
                id
            }
        );
    }

    private static DatosMulta CalcularMulta(
        Reserva reserva,
        DateTime fechaFinalizacion)
    {
        var diasTotales =
            (
                reserva.FechaHastaOriginal.Date -
                reserva.FechaDesde.Date
            ).Days;

        var diasTranscurridos =
            (
                fechaFinalizacion.Date -
                reserva.FechaDesde.Date
            ).Days;

        var diasRestantes =
            (
                reserva.FechaHastaOriginal.Date -
                fechaFinalizacion.Date
            ).Days;

        var porcentajeMulta =
            diasTranscurridos <
            diasTotales / 2.0
                ? 50m
                : 25m;

        var montoMulta =
            Math.Round(
                reserva.MontoPorDia *
                diasRestantes *
                porcentajeMulta /
                100m,
                2,
                MidpointRounding.AwayFromZero
            );

        return new DatosMulta
        {
            DiasRestantes =
                diasRestantes,

            PorcentajeMulta =
                porcentajeMulta,

            MontoMulta =
                montoMulta
        };
    }

    private sealed class DatosMulta
    {
        public int DiasRestantes { get; init; }

        public decimal PorcentajeMulta { get; init; }

        public decimal MontoMulta { get; init; }
    }
}
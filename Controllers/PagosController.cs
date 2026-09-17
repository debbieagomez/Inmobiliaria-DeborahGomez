using System.Security.Claims;
using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

[Authorize]
public class PagosController : Controller
{
    private readonly IRepositorioPago repositorioPago;
    private readonly IRepositorioReserva repositorioReserva;

    public PagosController(
        IRepositorioPago repositorioPago,
        IRepositorioReserva repositorioReserva)
    {
        this.repositorioPago = repositorioPago;
        this.repositorioReserva = repositorioReserva;
    }

    [HttpGet]
    public IActionResult Index(
        int reservaId,
        string? busqueda,
        int pagina = 1)
    {
        const int tamPagina = 10;

        if (reservaId <= 0)
        {
            return BadRequest();
        }

        var reserva =
            repositorioReserva.ObtenerPorId(reservaId);

        if (reserva == null)
        {
            return NotFound();
        }

        if (pagina < 1)
        {
            pagina = 1;
        }

        var cantidad =
            repositorioPago.ObtenerCantidadPorReserva(
                reservaId,
                busqueda
            );

        var totalPaginas =
            cantidad == 0
                ? 1
                : (int)Math.Ceiling(
                    (double)cantidad / tamPagina
                );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var pagos =
            repositorioPago.ObtenerPorReserva(
                reservaId,
                busqueda,
                pagina,
                tamPagina
            );

        ViewBag.Reserva = reserva;
        ViewBag.ReservaId = reservaId;
        ViewBag.Busqueda = busqueda;
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.Cantidad = cantidad;

        return View(pagos);
    }

    [HttpGet]
    public IActionResult Crear(int reservaId)
    {
        if (reservaId <= 0)
        {
            return BadRequest();
        }

        var reserva =
            repositorioReserva.ObtenerPorId(reservaId);

        if (reserva == null)
        {
            return NotFound();
        }

        var pago = new Pago
        {
            ReservaId = reservaId,
            FechaPago = DateTime.Now,
            Anulado = false,
            FechaAnulacion = null,
            UsuarioAnuladorId = null
        };

        ViewBag.Reserva = reserva;

        return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(Pago pago)
    {
        var reserva =
            repositorioReserva.ObtenerPorId(
                pago.ReservaId
            );

        if (reserva == null)
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

        pago.UsuarioCreadorId = usuarioId;
        pago.Anulado = false;
        pago.FechaAnulacion = null;
        pago.UsuarioAnuladorId = null;

        if (!ModelState.IsValid)
        {
            ViewBag.Reserva = reserva;

            return View(pago);
        }

        repositorioPago.Alta(pago);

        return RedirectToAction(
            nameof(Index),
            new
            {
                reservaId = pago.ReservaId
            }
        );
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var pago =
            repositorioPago.ObtenerPorId(id);

        if (pago == null)
        {
            return NotFound();
        }

        if (pago.Anulado)
        {
            TempData["Error"] =
                "No se puede editar un pago anulado.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    reservaId = pago.ReservaId
                }
            );
        }

        ViewBag.Reserva =
            repositorioReserva.ObtenerPorId(
                pago.ReservaId
            );

        return View(pago);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(
        int id,
        string concepto)
    {
        var pago =
            repositorioPago.ObtenerPorId(id);

        if (pago == null)
        {
            return NotFound();
        }

        if (pago.Anulado)
        {
            TempData["Error"] =
                "No se puede editar un pago anulado.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    reservaId = pago.ReservaId
                }
            );
        }

        concepto = concepto?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(concepto))
        {
            ModelState.AddModelError(
                "concepto",
                "El concepto es obligatorio."
            );

            ViewBag.Reserva =
                repositorioReserva.ObtenerPorId(
                    pago.ReservaId
                );

            return View(pago);
        }

        if (concepto.Length > 100)
        {
            ModelState.AddModelError(
                "concepto",
                "El concepto no puede superar los 100 caracteres."
            );

            ViewBag.Reserva =
                repositorioReserva.ObtenerPorId(
                    pago.ReservaId
                );

            return View(pago);
        }

        pago.Concepto = concepto;

        var resultado =
            repositorioPago.Modificacion(pago);

        if (resultado == 0)
        {
            TempData["Error"] =
                "No se pudo modificar el pago.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    reservaId = pago.ReservaId
                }
            );
        }

        return RedirectToAction(
            nameof(Index),
            new
            {
                reservaId = pago.ReservaId
            }
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult Anular(int id)
    {
        var pago =
            repositorioPago.ObtenerPorId(id);

        if (pago == null)
        {
            return NotFound();
        }

        if (pago.Anulado)
        {
            return RedirectToAction(
                nameof(Index),
                new
                {
                    reservaId = pago.ReservaId
                }
            );
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

        var resultado =
            repositorioPago.Anular(
                pago.IdPago,
                usuarioId
            );

        if (resultado == 0)
        {
            TempData["Error"] =
                "No se pudo anular el pago.";

            return RedirectToAction(
                nameof(Index),
                new
                {
                    reservaId = pago.ReservaId
                }
            );
        }

        return RedirectToAction(
            nameof(Index),
            new
            {
                reservaId = pago.ReservaId
            }
        );
    }

    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var pago =
            repositorioPago.ObtenerPorId(id);

        if (pago == null)
        {
            return NotFound();
        }

        return View(pago);
    }
}
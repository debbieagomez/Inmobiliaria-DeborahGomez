using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class ReservasController : ABMController<Reserva>
{
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioInquilino repositorioInquilino;

    public ReservasController(
        IRepositorioReserva repositorio,
        IRepositorioInmueble repositorioInmueble,
        IRepositorioInquilino repositorioInquilino)
        : base(repositorio)
    {
        this.repositorioInmueble = repositorioInmueble;
        this.repositorioInquilino = repositorioInquilino;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.Inmuebles = repositorioInmueble.ObtenerLista(tamPagina: 1000);
        ViewBag.Inquilinos = repositorioInquilino.ObtenerLista(tamPagina: 1000);

        base.OnActionExecuting(context);
    }

    public IActionResult Detalle(int id)
    {
        var Reserva = repositorio.ObtenerPorId(id);
        if (Reserva == null) return NotFound();

        return View(Reserva);
    }
}
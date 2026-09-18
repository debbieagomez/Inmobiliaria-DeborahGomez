using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Controllers;

[Authorize]
public abstract class ABMController<T> : Controller
{
    protected readonly IRepositorio<T> repositorio;

    protected ABMController(IRepositorio<T> repositorio)
    {
        this.repositorio = repositorio;
    }

    private string ClaveError =>
        $"Error_{ControllerContext.ActionDescriptor.ControllerName}";

    private void MostrarError(string mensaje)
    {
        TempData[ClaveError] = mensaje;
    }

    public virtual IActionResult Index(string? busqueda, int pagina = 1)
    {
        const int tamPagina = 10;

        if (pagina < 1)
        {
            pagina = 1;
        }

        var cantidad = repositorio.ObtenerCantidad(busqueda);

        var totalPaginas = cantidad == 0
            ? 1
            : (int)Math.Ceiling((double)cantidad / tamPagina);

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var lista = repositorio.ObtenerLista(busqueda, pagina, tamPagina);

        ViewBag.Busqueda = busqueda;
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.Cantidad = cantidad;

        return View(lista);
    }

    [HttpGet]
    public virtual IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual IActionResult Crear(T entidad)
    {
        if (entidad is Propietario propietario)
        {
            var repositorioPropietario = (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.ExisteDni(propietario.Dni))
            {
                ModelState.AddModelError("Dni", "El DNI ya existe.");
            }

            if (propietario.Email != null && repositorioPropietario.ExisteEmail(propietario.Email))
            {
                ModelState.AddModelError("Email", "El email ya existe.");
            }
        }
        else if (entidad is Inquilino inquilino)
        {
            var repositorioInquilino = (IRepositorioInquilino)repositorio;

            if (repositorioInquilino.ExisteDni(inquilino.Dni))
            {
                ModelState.AddModelError("Dni", "El DNI ya existe.");
            }

            if (inquilino.Email != null && repositorioInquilino.ExisteEmail(inquilino.Email))
            {
                ModelState.AddModelError("Email", "El email ya existe.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(entidad);
        }

        try
        {
            repositorio.Alta(entidad);
        }
        catch (MySqlException)
        {
            MostrarError("No se pudo guardar el registro. Verifique los datos e inténtelo nuevamente.");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual IActionResult Editar(int id)
    {
        var entidad = repositorio.ObtenerPorId(id);

        if (entidad == null)
        {
            return NotFound();
        }

        return View(entidad);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual IActionResult Editar(T entidad)
    {
        if (entidad is Propietario propietario)
        {
            var repositorioPropietario = (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.ExisteDni(propietario.Dni, propietario.IdPropietario))
            {
                ModelState.AddModelError("Dni", "El DNI ya existe.");
            }

            if (propietario.Email != null && repositorioPropietario.ExisteEmail(propietario.Email, propietario.IdPropietario))
            {
                ModelState.AddModelError("Email", "El email ya existe.");
            }
        }
        else if (entidad is Inquilino inquilino)
        {
            var repositorioInquilino = (IRepositorioInquilino)repositorio;

            if (repositorioInquilino.ExisteDni(inquilino.Dni, inquilino.IdInquilino))
            {
                ModelState.AddModelError("Dni", "El DNI ya existe.");
            }

            if (inquilino.Email != null && repositorioInquilino.ExisteEmail(inquilino.Email, inquilino.IdInquilino))
            {
                ModelState.AddModelError("Email", "El email ya existe.");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(entidad);
        }

        try
        {
            repositorio.Modificacion(entidad);
        }
        catch (MySqlException)
        {
            MostrarError("No se pudo modificar el registro. Verifique los datos e inténtelo nuevamente.");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var entidad = repositorio.ObtenerPorId(id);

        if (entidad == null)
        {
            return NotFound();
        }

        return View(entidad);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public IActionResult EliminarConfirmado(int id)
    {
        var entidad = repositorio.ObtenerPorId(id);

        if (entidad == null)
        {
            return NotFound();
        }

        if (entidad is Propietario propietario)
        {
            var repositorioPropietario = (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.TieneInmuebles(propietario.IdPropietario))
            {
                MostrarError("No se puede eliminar el propietario porque tiene inmuebles asociados.");
                return RedirectToAction(nameof(Index));
            }
        }

        if (entidad is Reserva reserva)
        {
            var repositorioReserva = (IRepositorioReserva)repositorio;

            if (repositorioReserva.TienePagos(reserva.IdReserva))
            {
                MostrarError("No se puede eliminar la reserva porque tiene pagos asociados. Los pagos deben conservarse como historial.");
                return RedirectToAction(nameof(Index));
            }
        }

        try
        {
            repositorio.Baja(id);
        }
        catch (MySqlException ex) when (ex.Number == 1451)
        {
            var mensaje = entidad switch
            {
                Inquilino => "No se puede eliminar el inquilino porque tiene reservas asociadas.",
                Inmueble => "No se puede eliminar el inmueble porque tiene reservas asociadas.",
                TipoInmueble => "No se puede eliminar el tipo de inmueble porque tiene inmuebles asociados.",
                Usuario => "No se puede eliminar el usuario porque tiene registros asociados.",
                _ => "No se puede eliminar el registro porque tiene datos asociados."
            };

            MostrarError(mensaje);
            return RedirectToAction(nameof(Index));
        }
        catch (MySqlException)
        {
            MostrarError("No se pudo eliminar el registro. Verifique los datos e inténtelo nuevamente.");
            return RedirectToAction(nameof(Index));
        }

        return RedirectToAction(nameof(Index));
    }
}

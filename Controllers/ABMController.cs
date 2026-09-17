using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

[Authorize]
public abstract class ABMController<T> : Controller
{
    protected readonly IRepositorio<T> repositorio;

    protected ABMController(IRepositorio<T> repositorio)
    {
        this.repositorio = repositorio;
    }

    public IActionResult Index(
        string? busqueda,
        int pagina = 1)
    {
        const int tamPagina = 10;

        if (pagina < 1)
        {
            pagina = 1;
        }

        var cantidad =
            repositorio.ObtenerCantidad(busqueda);

        var totalPaginas = cantidad == 0
            ? 1
            : (int)Math.Ceiling(
                (double)cantidad / tamPagina
            );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var lista =
            repositorio.ObtenerLista(
                busqueda,
                pagina,
                tamPagina
            );

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
            var repositorioPropietario =
                (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.ExisteDni(
                propietario.Dni))
            {
                ModelState.AddModelError(
                    "Dni",
                    "El Dni ya existe"
                );
            }

            if (propietario.Email != null)
            {
                if (repositorioPropietario.ExisteEmail(
                    propietario.Email))
                {
                    ModelState.AddModelError(
                        "Email",
                        "El email ya existe"
                    );
                }
            }
        }
        else if (entidad is Inquilino inquilino)
        {
            var repositorioInquilino =
                (IRepositorioInquilino)repositorio;

            if (repositorioInquilino.ExisteDni(
                inquilino.Dni))
            {
                ModelState.AddModelError(
                    "Dni",
                    "El Dni ya existe"
                );
            }

            if (inquilino.Email != null)
            {
                if (repositorioInquilino.ExisteEmail(
                    inquilino.Email))
                {
                    ModelState.AddModelError(
                        "Email",
                        "El email ya existe"
                    );
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(entidad);
        }

        repositorio.Alta(entidad);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public virtual IActionResult Editar(int id)
    {
        var entidad =
            repositorio.ObtenerPorId(id);

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
            var repositorioPropietario =
                (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.ExisteDni(
                propietario.Dni,
                propietario.IdPropietario))
            {
                ModelState.AddModelError(
                    "Dni",
                    "El Dni ya existe"
                );
            }

            if (propietario.Email != null)
            {
                if (repositorioPropietario.ExisteEmail(
                    propietario.Email,
                    propietario.IdPropietario))
                {
                    ModelState.AddModelError(
                        "Email",
                        "El email ya existe"
                    );
                }
            }
        }
        else if (entidad is Inquilino inquilino)
        {
            var repositorioInquilino =
                (IRepositorioInquilino)repositorio;

            if (repositorioInquilino.ExisteDni(
                inquilino.Dni,
                inquilino.IdInquilino))
            {
                ModelState.AddModelError(
                    "Dni",
                    "El Dni ya existe"
                );
            }

            if (inquilino.Email != null)
            {
                if (repositorioInquilino.ExisteEmail(
                    inquilino.Email,
                    inquilino.IdInquilino))
                {
                    ModelState.AddModelError(
                        "Email",
                        "El email ya existe"
                    );
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(entidad);
        }

        repositorio.Modificacion(entidad);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var entidad =
            repositorio.ObtenerPorId(id);

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
        var entidad =
            repositorio.ObtenerPorId(id);

        if (entidad == null)
        {
            return NotFound();
        }

        if (entidad is Propietario propietario)
        {
            var repositorioPropietario =
                (IRepositorioPropietario)repositorio;

            if (repositorioPropietario.TieneInmuebles(
                propietario.IdPropietario))
            {
                TempData["Error"] =
                    "No se puede eliminar el propietario porque tiene inmuebles asociados.";

                return RedirectToAction(nameof(Index));
            }
        }

        repositorio.Baja(id);

        return RedirectToAction(nameof(Index));
    }
}


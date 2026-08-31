using Inmobiliaria_DeborahGomez.Models;
using Microsoft.AspNetCore.Mvc;



namespace Inmobiliaria_DeborahGomez.Repositories;

public abstract class ABMController<T> : Controller
{
    protected readonly IRepositorio<T> repositorio;

    protected ABMController (IRepositorio<T> repositorio) {

        this.repositorio = repositorio;
        
    }

    //metodos------------------------
    //Indice
    public IActionResult Index(string? busqueda, int pagina = 1)
    {
        var lista = repositorio.ObtenerLista(busqueda, pagina, 10);

        return View(lista);
    }

    //GET de crear
    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    //POST de crear
    [HttpPost]
    public IActionResult Crear(T entidad)
    {
        
        if (entidad is Propietario propietario) 
        {
            if (repositorio.ExisteDni(propietario.Dni))
            {
                ModelState.AddModelError("Dni", "El Dni ya existe");
            }
            if (propietario.Email != null)
            {
                if (repositorio.ExisteEmail(propietario.Email))
                {
                    ModelState.AddModelError("Email", "El email ya existe");
                }
            } 
        } 
        else if (entidad is Inquilino inquilino)
        {
            if (repositorio.ExisteDni(inquilino.Dni))
            {
                ModelState.AddModelError("Dni", "El Dni ya existe");
            }
            if (inquilino.Email != null)
            {
                if (repositorio.ExisteEmail(inquilino.Email))
                {
                    ModelState.AddModelError("Email", "El email ya existe");
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

    //GET de Editar
    [HttpGet]
    public IActionResult Editar(int id)
    {
        var entidad = repositorio.ObtenerPorId(id);

        if (entidad == null)
        {
            return NotFound();
        }

        return View(entidad);

    }

    //POST de Editar
    [HttpPost]
    public IActionResult Editar(T entidad)
    {

        if (entidad is Propietario propietario) 
        {
            if (repositorio.ExisteDni(propietario.Dni))
            {
                ModelState.AddModelError("Dni", "El Dni ya existe");
            }
            if (propietario.Email != null)
            {
                if (repositorio.ExisteEmail(propietario.Email))
                {
                    ModelState.AddModelError("Email", "El email ya existe");
                }
            } 
        } 
        else if (entidad is Inquilino inquilino)
        {
            if (repositorio.ExisteDni(inquilino.Dni))
            {
                ModelState.AddModelError("Dni", "El Dni ya existe");
            }
            if (inquilino.Email != null)
            {
                if (repositorio.ExisteEmail(inquilino.Email))
                {
                    ModelState.AddModelError("Email", "El email ya existe");
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

    //GET de Eliminar
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

    //POST de Eliminar (se necesita confirmar desde el GET)
    [HttpPost]
    public IActionResult EliminarConfirmado(int id)
    {
        
        repositorio.Baja(id);
        return RedirectToAction(nameof(Index));

    }


}
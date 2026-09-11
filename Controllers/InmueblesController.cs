using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class InmueblesController : ABMController<Inmueble>
{
    private readonly IRepositorioPropietario repositorioPropietario;
    private readonly IRepositorioTipoInmueble repositorioTipoInmueble;

    public InmueblesController(
        IRepositorioInmueble repositorio,
        IRepositorioPropietario repositorioPropietario,
        IRepositorioTipoInmueble repositorioTipo) : base(repositorio)
    {
        this.repositorioPropietario = repositorioPropietario;
        this.repositorioTipoInmueble = repositorioTipo;   
    }

    public new IActionResult Crear()
    {
        CargarListasDesplegables();
        return View();
    }

    public new IActionResult Editar(int id)
    {
        var inmueble = repositorio.ObtenerPorId(id); 
        if (inmueble == null) return NotFound();

        CargarListasDesplegables();   
        return View(inmueble);        
    }

    private void CargarListasDesplegables()
    {
        ViewBag.Propietarios = repositorioPropietario.ObtenerLista(tamPagina: 1000);
        ViewBag.Tipos = repositorioTipoInmueble.ObtenerLista(tamPagina: 1000);
    }
}
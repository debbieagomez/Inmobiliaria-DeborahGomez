using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.Propietarios = repositorioPropietario.ObtenerLista(tamPagina: 1000);
        ViewBag.Tipos = repositorioTipoInmueble.ObtenerLista(tamPagina: 1000);
        base.OnActionExecuting(context);
    }
    
}
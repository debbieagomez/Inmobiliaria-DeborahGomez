using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class TipoInmuebleController : ABMController<TipoInmueble>
{
    public TipoInmuebleController (IRepositorioTipoInmueble repositorio) : base(repositorio)
    {
    }

}
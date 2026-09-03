using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
 
namespace Inmobiliaria_DeborahGomez.Controllers;
 
public class InmueblesController : ABMController<Inmueble>
{
 
    public InmueblesController (IRepositorioInmueble repositorio) : base(repositorio)
    {
 
    }
 
}
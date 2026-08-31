using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class PropietariosController : ABMController<Propietario>
{
    
    public PropietariosController (IRepositorioPropietario repositorio) : base(repositorio)
    {
        
    }

}

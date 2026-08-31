using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class InquilinosController : ABMController<Inquilino>
{
    
    public InquilinosController (IRepositorioInquilino repositorio) : base(repositorio)
    {
        
    }

}
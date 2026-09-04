using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class ReservasController : ABMController<Reserva>
{
    public ReservasController(IRepositorioReserva repositorio)
      : base(repositorio)
    {
    }
}
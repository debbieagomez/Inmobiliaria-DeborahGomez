using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioReserva : IRepositorio<Reserva>
{
    bool ExisteSolapamiento(
        int inmuebleId,
        DateTime fechaDesde,
        DateTime fechaHasta,
        int? idReservaExcluir = null
    );

    IList<Inmueble> BuscarDisponibles(
        DateTime fechaDesde,
        DateTime fechaHasta,
        int? cupo = null,
        int? tipoInmuebleId = null,
        decimal? precioMaximo = null
    );
}
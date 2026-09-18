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

    int AltaConSena(
        Reserva reserva,
        decimal porcentajeSena
    );

    int FinalizarAnticipadamente(
        int idReserva,
        DateTime fechaFinalizacion,
        int usuarioFinalizadorId
    );

    decimal CalcularMontoMulta(
        int idReserva,
        DateTime fechaFinalizacion
    );

    bool TienePagos(
        int idReserva
    );

    IList<Reserva> ObtenerVigentes(
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadVigentes();

    IList<Reserva> ObtenerQueTerminanEn(
        int dias,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadQueTerminanEn(
        int dias
    );
}
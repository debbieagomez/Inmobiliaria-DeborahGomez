using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioInmueble : IRepositorio<Inmueble>
{
    IList<Inmueble> ObtenerPorDisponibilidad(
        bool? disponible,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadPorDisponibilidad(
        bool? disponible
    );

    IList<Inmueble> ObtenerPorPropietario(
        int propietarioId,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadPorPropietario(
        int propietarioId
    );

    IList<InmuebleConReservas> ObtenerMasReservados(
        int dias = 365,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadMasReservados(
        int dias = 365
    );

    IList<Inmueble> ObtenerSinReservas(
        int dias,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadSinReservas(
        int dias
    );

    IList<Inmueble> ObtenerDisponiblesEntreFechas(
        DateTime fechaDesde,
        DateTime fechaHasta,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadDisponiblesEntreFechas(
        DateTime fechaDesde,
        DateTime fechaHasta
    );

    bool TieneReservas(
        int inmuebleId
    );
}
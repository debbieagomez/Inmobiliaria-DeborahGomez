using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioInmueble : IRepositorio<Inmueble>
{
    // Listar inmuebles y su dueño, con filtro opcional por disponibilidad
    IList<Inmueble> ObtenerPorDisponibilidad(bool? disponible, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadPorDisponibilidad(bool? disponible);
    // Informe 2: Listar inmuebles de un propietario específico
    IList<Inmueble> ObtenerPorPropietario(int propietarioId, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadPorPropietario(int propietarioId);

    // Informe 3: Inmuebles más reservados en los últimos X días (default 365)
    IList<InmuebleConReservas> ObtenerMasReservados(int dias = 365, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadMasReservados(int dias = 365);

    // Informe 4: Inmuebles SIN reservas en los últimos X días
    IList<Inmueble> ObtenerSinReservas(int dias, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadSinReservas(int dias);

    // Informe 5: Inmuebles NO ocupados (disponibles) entre dos fechas dadas
    IList<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fechaDesde, DateTime fechaHasta, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadDisponiblesEntreFechas(DateTime fechaDesde, DateTime fechaHasta);
}
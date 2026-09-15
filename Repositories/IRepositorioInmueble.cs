using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioInmueble : IRepositorio<Inmueble>
{
    // Listar inmuebles y su dueño, con filtro opcional por disponibilidad
    IList<Inmueble> ObtenerPorDisponibilidad(bool? disponible, int pagina = 1, int tamPagina = 10);
    int ObtenerCantidadPorDisponibilidad(bool? disponible);
}
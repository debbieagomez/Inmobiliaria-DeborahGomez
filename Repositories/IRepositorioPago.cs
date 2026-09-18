using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioPago : IRepositorio<Pago>
{
    IList<Pago> ObtenerPorReserva(
        int reservaId,
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10
    );

    int ObtenerCantidadPorReserva(
        int reservaId,
        string? busqueda = null
    );

    int Anular(
        int idPago,
        int usuarioAnuladorId
    );

    bool ExistePagoMulta(
        int reservaId,
        decimal importe
    );
}
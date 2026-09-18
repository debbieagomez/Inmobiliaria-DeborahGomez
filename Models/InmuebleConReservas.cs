namespace Inmobiliaria_DeborahGomez.Models;

public class InmuebleConReservas
{
    public Inmueble Inmueble { get; set; } = null!;

    public int CantidadReservas { get; set; }

    public DateTime UltimaFecha { get; set; }
}
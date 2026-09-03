namespace Inmobiliaria_DeborahGomez.Models;

public class Reserva
{
    public int IdReserva { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
    public DateTime FechaHastaOriginal { get; set; }
    public decimal MontoPorDia { get; set; }
    public bool Finalizada { get; set; }
    public DateTime? FechaFinalizacionAnticipada { get; set; }
    public decimal? MontoMulta { get; set; }
    public int InmuebleId { get; set; }
    public int InquilinoId { get; set; }
    public int UsuarioCreadorId { get; set; }
    public int? UsuarioFinalizadorId { get; set; }

}
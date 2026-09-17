using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Reserva : IValidatableObject
{
    public int IdReserva { get; set; }

    [Display(Name = "Fecha de inicio")]
    [Required]
    public DateTime FechaDesde { get; set; }

    [Display(Name = "Fecha de finalización")]
    [Required]
    public DateTime FechaHasta { get; set; }

    public DateTime FechaHastaOriginal { get; set; }

    [Range(0.01, double.MaxValue)]
    [Required]
    public decimal MontoPorDia { get; set; }

    public bool Finalizada { get; set; }

    public DateTime? FechaFinalizacionAnticipada { get; set; }

    public decimal? MontoMulta { get; set; }

    [Range(1, int.MaxValue)]
    public int InmuebleId { get; set; }

    [Range(1, int.MaxValue)]
    public int InquilinoId { get; set; }

    public int UsuarioCreadorId { get; set; }

    public int? UsuarioFinalizadorId { get; set; }

    public string? DireccionInmueble { get; set; }

    public string? NombreInquilino { get; set; }

    public decimal? MontoSena { get; set; }

    public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
    {
        if (FechaHasta < FechaDesde)
        {
            yield return new ValidationResult(
                "La fecha de finalización no puede ser anterior a la fecha de inicio.",
                new[] { nameof(FechaHasta) }
            );
        }
    }
}
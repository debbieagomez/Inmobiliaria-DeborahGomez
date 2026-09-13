using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Reserva : IValidatableObject
{
    public int IdReserva { get; set; }

    [Display(Name = "Fecha de inicio")]
    [Required(ErrorMessage = "Debe indicar la fecha de inicio.")]
    public DateTime FechaDesde { get; set; }

    [Display(Name = "Fecha de finalización")]
    [Required(ErrorMessage = "Debe indicar la fecha de finalización.")]
    public DateTime FechaHasta { get; set; }

    [Display(Name = "Fecha de finalización original")]
    public DateTime FechaHastaOriginal { get; set; }

    [Display(Name = "Monto por día")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto por día debe ser mayor que 0.")]
    [Required(ErrorMessage = "Debe indicar un monto de la reserva.")]
    public decimal MontoPorDia { get; set; }

    public bool Finalizada { get; set; }

    public DateTime? FechaFinalizacionAnticipada { get; set; }

    public decimal? MontoMulta { get; set; }

    [Display(Name = "Inmueble")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inmueble.")]
    public int InmuebleId { get; set; }

    [Display(Name = "Inquilino")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un inquilino.")]
    public int InquilinoId { get; set; }

    public int UsuarioCreadorId { get; set; }

    public int? UsuarioFinalizadorId { get; set; }


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
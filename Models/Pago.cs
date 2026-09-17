using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Pago
{
    public int IdPago { get; set; }

    [Required(ErrorMessage = "El concepto es obligatorio.")]
    [StringLength(
        100,
        ErrorMessage = "El concepto no puede superar los 100 caracteres."
    )]
    [Display(Name = "Concepto")]
    public string Concepto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
    [Display(Name = "Fecha de pago")]
    public DateTime FechaPago { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "El importe es obligatorio.")]
    [Range(
        0.01,
        double.MaxValue,
        ErrorMessage = "El importe debe ser mayor a 0."
    )]
    [Display(Name = "Importe")]
    public decimal Importe { get; set; }

    [Display(Name = "Anulado")]
    public bool Anulado { get; set; }

    [Display(Name = "Fecha de anulación")]
    public DateTime? FechaAnulacion { get; set; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "Debe indicar una reserva."
    )]
    [Display(Name = "Reserva")]
    public int ReservaId { get; set; }

    public int UsuarioCreadorId { get; set; }

    public int? UsuarioAnuladorId { get; set; }


    public string? DireccionInmueble { get; set; }

    public string? NombreInquilino { get; set; }
}
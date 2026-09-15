using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Propietario
{
    public int IdPropietario { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(45, ErrorMessage = "El nombre no puede superar los 45 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(45, ErrorMessage = "El apellido no puede superar los 45 caracteres.")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El DNI es obligatorio.")]
    [StringLength(45, ErrorMessage = "El DNI no puede superar los 45 caracteres.")]
    public string Dni { get; set; } = string.Empty;

    [StringLength(45, ErrorMessage = "El teléfono no puede superar los 45 caracteres.")]
    public string? Telefono { get; set; }

    [StringLength(45, ErrorMessage = "El email no puede superar los 45 caracteres.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    public string? Email { get; set; }
}
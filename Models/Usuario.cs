using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Usuario
{
    public int IdUsuario { get; set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no es válido.")]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public string Rol { get; set; } = string.Empty;

    public string? Avatar { get; set; }
}
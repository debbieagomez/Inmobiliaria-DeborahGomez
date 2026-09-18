using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria_DeborahGomez.Models;

public class Inmueble
{
    public int IdInmueble { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El cupo debe ser mayor a 0.")]
    public int Cupo { get; set; }

    [Range(-90, 90, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
    public decimal? Latitud { get; set; }

    [Range(-180, 180, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
    public decimal? Longitud { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio por día debe ser mayor a 0.")]
    public decimal PrecioPorDia { get; set; }

    [Range(0, 100, ErrorMessage = "El porcentaje de seña debe estar entre 0 y 100.")]
    public decimal PorcentajeSenia { get; set; }

    public bool Disponible { get; set; } = true;

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un propietario.")]
    public int PropietarioId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un tipo de inmueble.")]
    public int TipoInmuebleId { get; set; }

    public string? propietarioNombre { get; set; }

    public string? TipoNombre { get; set; }

    public IList<ImagenInmueble> Imagenes { get; set; }
        = new List<ImagenInmueble>();
}
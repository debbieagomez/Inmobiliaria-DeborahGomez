namespace Models;

public class Inmueble
{
    public int IdInmueble { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public int Cupo { get; set;}
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public decimal PrecioPorDia { get; set; }
    public decimal PorcentajeSenia { get; set; }
    public bool Disponible { get; set; } = true; //si no pongo true, csharp arranca en false
    public string? ImagenPortadaUrl { get; set; }
    public int PropietarioId { get; set; }
    public int TipoInmuebleId { get; set; }
    public string? propietarioNombre { get; set; }
    public string? TipoNombre { get; set; }
}
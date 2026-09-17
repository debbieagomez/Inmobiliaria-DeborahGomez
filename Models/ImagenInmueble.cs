namespace Inmobiliaria_DeborahGomez.Models;

public class ImagenInmueble
{
    public int IdImagenInmueble { get; set; }

    public int InmuebleId { get; set; }

    public string Url { get; set; } = string.Empty;

    public bool EsPortada { get; set; }
}
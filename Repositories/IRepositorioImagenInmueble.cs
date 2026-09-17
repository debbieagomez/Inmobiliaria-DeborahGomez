using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioImagenInmueble
{
    IList<ImagenInmueble> ObtenerPorInmueble(int inmuebleId);

    ImagenInmueble? ObtenerPorId(int id);

    int Alta(ImagenInmueble imagen);

    int Baja(int id);

    int EstablecerPortada(int inmuebleId, int imagenId);

    bool TienePortada(int inmuebleId);
}
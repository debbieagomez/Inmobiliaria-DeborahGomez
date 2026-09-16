using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;

public interface IRepositorioUsuario : IRepositorio<Usuario>
{
    Usuario? ObtenerPorEmail(string email);

    bool ExisteEmail(string email, int idExcluir = 0);
}
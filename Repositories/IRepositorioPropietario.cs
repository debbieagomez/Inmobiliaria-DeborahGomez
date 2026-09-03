using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;


public interface IRepositorioPropietario : IRepositorio<Propietario>
{
    
    bool ExisteDni(string dni, int idExcluir = 0);
    bool ExisteEmail(string email, int idExcluir = 0);
}
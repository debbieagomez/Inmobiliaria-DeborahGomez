
using Inmobiliaria_DeborahGomez.Models;

namespace Inmobiliaria_DeborahGomez.Repositories;


public interface IRepositorioInquilino : IRepositorio<Inquilino>
{
    
    bool ExisteDni(string dni, int idExcluir = 0);
    bool ExisteEmail(string email, int idExcluir = 0);
}
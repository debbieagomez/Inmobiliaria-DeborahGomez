using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

[Authorize(Roles = "Administrador")]
public class UsuariosController : Controller
{
    private readonly IRepositorioUsuario repositorioUsuario;
    private readonly IPasswordHasher<Usuario> passwordHasher;

    public UsuariosController(
        IRepositorioUsuario repositorioUsuario,
        IPasswordHasher<Usuario> passwordHasher)
    {
        this.repositorioUsuario = repositorioUsuario;
        this.passwordHasher = passwordHasher;
    }

    public IActionResult Index(
        string? busqueda,
        int pagina = 1)
    {
        const int tamPagina = 10;

        if (pagina < 1)
        {
            pagina = 1;
        }

        var cantidad =
            repositorioUsuario.ObtenerCantidad(busqueda);

        var totalPaginas = cantidad == 0
            ? 1
            : (int)Math.Ceiling(
                (double)cantidad / tamPagina
            );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var usuarios =
            repositorioUsuario.ObtenerLista(
                busqueda,
                pagina,
                tamPagina
            );

        ViewBag.Busqueda = busqueda;
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.Cantidad = cantidad;

        return View(usuarios);
    }

    [HttpGet]
    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(
        Usuario usuario,
        string password)
    {
        if (repositorioUsuario.ExisteEmail(usuario.Email))
        {
            ModelState.AddModelError(
                "Email",
                "El email ya existe."
            );
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(
                "Password",
                "La contraseña es obligatoria."
            );
        }

        if (usuario.Rol != "Administrador"
            && usuario.Rol != "Empleado")
        {
            ModelState.AddModelError(
                "Rol",
                "El rol seleccionado no es válido."
            );
        }

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        usuario.PasswordHash =
            passwordHasher.HashPassword(
                usuario,
                password
            );

        repositorioUsuario.Alta(usuario);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var usuario =
            repositorioUsuario.ObtenerPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(
        Usuario usuario,
        string? password)
    {
        if (repositorioUsuario.ExisteEmail(
            usuario.Email,
            usuario.IdUsuario))
        {
            ModelState.AddModelError(
                "Email",
                "El email ya existe."
            );
        }

        if (usuario.Rol != "Administrador"
            && usuario.Rol != "Empleado")
        {
            ModelState.AddModelError(
                "Rol",
                "El rol seleccionado no es válido."
            );
        }

        var usuarioActual =
            repositorioUsuario.ObtenerPorId(
                usuario.IdUsuario
            );

        if (usuarioActual == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(usuario);
        }

        usuario.PasswordHash =
            usuarioActual.PasswordHash;

        if (!string.IsNullOrWhiteSpace(password))
        {
            usuario.PasswordHash =
                passwordHasher.HashPassword(
                    usuario,
                    password
                );
        }

        repositorioUsuario.Modificacion(usuario);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        var usuario =
            repositorioUsuario.ObtenerPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarConfirmado(int id)
    {
        var usuario =
            repositorioUsuario.ObtenerPorId(id);

        if (usuario == null)
        {
            return NotFound();
        }

        repositorioUsuario.Baja(id);

        return RedirectToAction(nameof(Index));
    }
}
using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

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

    [Authorize(Roles = "Administrador")]
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
        var cantidad = repositorioUsuario.ObtenerCantidad();

        // Si ya existe al menos un usuario,
        // solamente un administrador puede crear otros usuarios.
        if (cantidad > 0 && !User.IsInRole("Administrador"))
        {
            return Forbid();
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(
        Usuario usuario,
        string password)
    {
        var cantidad = repositorioUsuario.ObtenerCantidad();

        // Si ya existen usuarios, solamente un administrador
        // puede crear otro usuario.
        if (cantidad > 0 && !User.IsInRole("Administrador"))
        {
            return Forbid();
        }

        // El primer usuario SIEMPRE debe ser Administrador.
        if (cantidad == 0)
        {
            usuario.Rol = "Administrador";
        }

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
            ViewBag.PrimerUsuario = cantidad == 0;
            return View(usuario);
        }

        // La contraseña nunca se guarda en texto plano.
        usuario.PasswordHash =
            passwordHasher.HashPassword(
                usuario,
                password
            );

        repositorioUsuario.Alta(usuario);

        // Si acabamos de crear el primer usuario,
        // todavía no está autenticado.
        if (cantidad == 0)
        {
            TempData["Mensaje"] =
                "Administrador creado correctamente. Ahora puede iniciar sesión.";

            return RedirectToAction(
                "Login",
                "Cuenta"
            );
        }

        // Si lo creó un administrador autenticado,
        // volvemos al listado.
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Administrador")]
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

    [Authorize(Roles = "Administrador")]
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

        // Mantener la contraseña actual si no se escribió una nueva.
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

    [Authorize(Roles = "Administrador")]
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

    [Authorize(Roles = "Administrador")]
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

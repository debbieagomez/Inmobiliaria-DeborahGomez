using System.Security.Claims;
using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class CuentaController : Controller
{
    private readonly IRepositorioUsuario repositorioUsuario;
    private readonly IPasswordHasher<Usuario> passwordHasher;

    public CuentaController(
        IRepositorioUsuario repositorioUsuario,
        IPasswordHasher<Usuario> passwordHasher)
    {
        this.repositorioUsuario = repositorioUsuario;
        this.passwordHasher = passwordHasher;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(
        string email,
        string password,
        string? returnUrl = null)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError(
                "Email",
                "El email es obligatorio."
            );
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError(
                "Password",
                "La contraseña es obligatoria."
            );
        }

        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        var usuario = repositorioUsuario.ObtenerPorEmail(
            email.Trim()
        );

        if (usuario == null)
        {
            ModelState.AddModelError(
                "",
                "Email o contraseña incorrectos."
            );

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            password
        );

        if (resultado == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError(
                "",
                "Email o contraseña incorrectos."
            );

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        await IniciarSesion(usuario);

        if (!string.IsNullOrWhiteSpace(returnUrl)
            && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction(
            "Index",
            "Home"
        );
    }

    [Authorize]
    [HttpGet]
    public IActionResult Perfil()
    {
        var idUsuario = ObtenerIdUsuarioActual();

        if (idUsuario == null)
        {
            return Challenge();
        }

        var usuario = repositorioUsuario.ObtenerPorId(
            idUsuario.Value
        );

        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Perfil(
        string email,
        string? passwordActual,
        string? passwordNueva,
        string? confirmarPassword)
    {
        var idUsuario = ObtenerIdUsuarioActual();

        if (idUsuario == null)
        {
            return Challenge();
        }

        var usuarioActual = repositorioUsuario.ObtenerPorId(
            idUsuario.Value
        );

        if (usuarioActual == null)
        {
            return NotFound();
        }

        email = email?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError(
                "email",
                "El email es obligatorio."
            );
        }
        else
        {
            if (repositorioUsuario.ExisteEmail(
                email,
                usuarioActual.IdUsuario))
            {
                ModelState.AddModelError(
                    "email",
                    "El email ya existe."
                );
            }
        }

        var quiereCambiarPassword =
            !string.IsNullOrWhiteSpace(passwordActual)
            || !string.IsNullOrWhiteSpace(passwordNueva)
            || !string.IsNullOrWhiteSpace(confirmarPassword);

        var nuevoPasswordHash = usuarioActual.PasswordHash;

        if (quiereCambiarPassword)
        {
            if (string.IsNullOrWhiteSpace(passwordActual))
            {
                ModelState.AddModelError(
                    "passwordActual",
                    "Debe ingresar su contraseña actual."
                );
            }
            else
            {
                var resultado =
                    passwordHasher.VerifyHashedPassword(
                        usuarioActual,
                        usuarioActual.PasswordHash,
                        passwordActual
                    );

                if (resultado == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError(
                        "passwordActual",
                        "La contraseña actual es incorrecta."
                    );
                }
            }

            if (string.IsNullOrWhiteSpace(passwordNueva))
            {
                ModelState.AddModelError(
                    "passwordNueva",
                    "La nueva contraseña es obligatoria."
                );
            }
            else if (passwordNueva.Length < 6)
            {
                ModelState.AddModelError(
                    "passwordNueva",
                    "La nueva contraseña debe tener al menos 6 caracteres."
                );
            }

            if (passwordNueva != confirmarPassword)
            {
                ModelState.AddModelError(
                    "confirmarPassword",
                    "Las contraseñas no coinciden."
                );
            }

            if (string.IsNullOrWhiteSpace(
                    passwordActual)
                == false
                && !string.IsNullOrWhiteSpace(
                    passwordNueva)
                && passwordNueva == confirmarPassword)
            {
                var resultado =
                    passwordHasher.VerifyHashedPassword(
                        usuarioActual,
                        usuarioActual.PasswordHash,
                        passwordActual
                    );

                if (resultado != PasswordVerificationResult.Failed)
                {
                    nuevoPasswordHash =
                        passwordHasher.HashPassword(
                            usuarioActual,
                            passwordNueva
                        );
                }
            }
        }

        if (!ModelState.IsValid)
        {
            var usuarioVista = new Usuario
            {
                IdUsuario = usuarioActual.IdUsuario,
                Email = email,
                PasswordHash = usuarioActual.PasswordHash,
                Rol = usuarioActual.Rol
            };

            return View(usuarioVista);
        }

        var usuarioModificado = new Usuario
        {
            IdUsuario = usuarioActual.IdUsuario,
            Email = email,
            PasswordHash = nuevoPasswordHash,
            Rol = usuarioActual.Rol
        };

        repositorioUsuario.Modificacion(
            usuarioModificado
        );

        await IniciarSesion(
            usuarioModificado
        );

        TempData["Mensaje"] =
            "Perfil actualizado correctamente.";

        return RedirectToAction(
            nameof(Perfil)
        );
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return RedirectToAction(
            "Index",
            "Home"
        );
    }

    [HttpGet]
    public IActionResult AccesoDenegado()
    {
        return View();
    }

    private int? ObtenerIdUsuarioActual()
    {
        var claim = User.FindFirstValue(
            ClaimTypes.NameIdentifier
        );

        if (int.TryParse(claim, out var id))
        {
            return id;
        }

        return null;
    }

    private ClaimsPrincipal CrearPrincipal(
        Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                usuario.IdUsuario.ToString()
            ),
            new Claim(
                ClaimTypes.Name,
                usuario.Email
            ),
            new Claim(
                ClaimTypes.Role,
                usuario.Rol
            )
        };

        var identidad = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return new ClaimsPrincipal(identidad);
    }

    private async Task IniciarSesion(
        Usuario usuario)
    {
        var principal = CrearPrincipal(usuario);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );
    }
}
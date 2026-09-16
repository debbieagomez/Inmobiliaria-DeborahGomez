using System.Security.Claims;
using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        var usuario = repositorioUsuario.ObtenerPorEmail(email);

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

        var principal = new ClaimsPrincipal(identidad);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal
        );

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
}
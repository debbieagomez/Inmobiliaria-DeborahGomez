using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Inmobiliaria_DeborahGomez.Controllers;

public class InmueblesController : ABMController<Inmueble>
{
    private readonly IRepositorioInmueble repositorioInmueble;
    private readonly IRepositorioPropietario repositorioPropietario;
    private readonly IRepositorioTipoInmueble repositorioTipoInmueble;
    private readonly IRepositorioImagenInmueble repositorioImagen;
    private readonly IWebHostEnvironment environment;

    public InmueblesController(
        IRepositorioInmueble repositorioInmueble,
        IRepositorioPropietario repositorioPropietario,
        IRepositorioTipoInmueble repositorioTipoInmueble,
        IRepositorioImagenInmueble repositorioImagen,
        IWebHostEnvironment environment)
        : base(repositorioInmueble)
    {
        this.repositorioInmueble = repositorioInmueble;
        this.repositorioPropietario = repositorioPropietario;
        this.repositorioTipoInmueble = repositorioTipoInmueble;
        this.repositorioImagen = repositorioImagen;
        this.environment = environment;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        ViewBag.Propietarios =
            repositorioPropietario.ObtenerLista(
                tamPagina: 1000
            );

        ViewBag.Tipos =
            repositorioTipoInmueble.ObtenerLista(
                tamPagina: 1000
            );

        base.OnActionExecuting(context);
    }

    public override IActionResult Index(
        string? busqueda,
        int pagina = 1)
    {
        const int tamPagina = 10;

        if (pagina < 1)
        {
            pagina = 1;
        }

        var cantidad =
            repositorioInmueble.ObtenerCantidad(
                busqueda
            );

        var totalPaginas = cantidad == 0
            ? 1
            : (int)Math.Ceiling(
                (double)cantidad / tamPagina
            );

        if (pagina > totalPaginas)
        {
            pagina = totalPaginas;
        }

        var lista =
            repositorioInmueble.ObtenerLista(
                busqueda,
                pagina,
                tamPagina
            );

        foreach (var inmueble in lista)
        {
            inmueble.Imagenes =
                repositorioImagen.ObtenerPorInmueble(
                    inmueble.IdInmueble
                );
        }

        ViewBag.Busqueda = busqueda;
        ViewBag.PaginaActual = pagina;
        ViewBag.TotalPaginas = totalPaginas;
        ViewBag.Cantidad = cantidad;

        return View(lista);
    }

    [HttpGet]
    public override IActionResult Crear()
    {
        return View(new Inmueble());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override IActionResult Crear(
        Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            return View(inmueble);
        }

        repositorioInmueble.Alta(inmueble);

        var inmuebles =
            repositorioInmueble.ObtenerLista(
                null,
                1,
                1000
            );

        var inmuebleCreado =
            inmuebles
                .Where(i =>
                    i.Direccion == inmueble.Direccion &&
                    i.PropietarioId == inmueble.PropietarioId &&
                    i.TipoInmuebleId == inmueble.TipoInmuebleId
                )
                .OrderByDescending(i => i.IdInmueble)
                .FirstOrDefault();

        if (inmuebleCreado == null)
        {
            TempData["Error"] =
                "El inmueble fue creado, pero no se pudo obtener su identificador.";

            return RedirectToAction(nameof(Index));
        }

        inmueble.IdInmueble =
            inmuebleCreado.IdInmueble;

        foreach (var archivo in Request.Form.Files)
        {
            GuardarImagen(
                inmueble.IdInmueble,
                archivo
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public override IActionResult Editar(int id)
    {
        var inmueble =
            repositorioInmueble.ObtenerPorId(id);

        if (inmueble == null)
        {
            return NotFound();
        }

        inmueble.Imagenes =
            repositorioImagen.ObtenerPorInmueble(id);

        return View(inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override IActionResult Editar(
        Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            inmueble.Imagenes =
                repositorioImagen.ObtenerPorInmueble(
                    inmueble.IdInmueble
                );

            return View(inmueble);
        }

        var existente =
            repositorioInmueble.ObtenerPorId(
                inmueble.IdInmueble
            );

        if (existente == null)
        {
            return NotFound();
        }

        repositorioInmueble.Modificacion(
            inmueble
        );

        foreach (var archivo in Request.Form.Files)
        {
            GuardarImagen(
                inmueble.IdInmueble,
                archivo
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Detalle(int id)
    {
        var inmueble =
            repositorioInmueble.ObtenerPorId(id);

        if (inmueble == null)
        {
            return NotFound();
        }

        inmueble.Imagenes =
            repositorioImagen.ObtenerPorInmueble(id);

        return View(inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EstablecerPortada(
        int idInmueble,
        int idImagen)
    {
        var inmueble =
            repositorioInmueble.ObtenerPorId(
                idInmueble
            );

        if (inmueble == null)
        {
            return NotFound();
        }

        var imagen =
            repositorioImagen.ObtenerPorId(
                idImagen
            );

        if (imagen == null ||
            imagen.InmuebleId != idInmueble)
        {
            return NotFound();
        }

        repositorioImagen.EstablecerPortada(
            idInmueble,
            idImagen
        );

        return RedirectToAction(
            nameof(Detalle),
            new { id = idInmueble }
        );
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarImagen(
        int idInmueble,
        int idImagen)
    {
        var inmueble =
            repositorioInmueble.ObtenerPorId(
                idInmueble
            );

        if (inmueble == null)
        {
            return NotFound();
        }

        var imagen =
            repositorioImagen.ObtenerPorId(
                idImagen
            );

        if (imagen == null ||
            imagen.InmuebleId != idInmueble)
        {
            return NotFound();
        }

        EliminarArchivoFisico(
            imagen.Url
        );

        repositorioImagen.Baja(
            idImagen
        );

        return RedirectToAction(
            nameof(Detalle),
            new { id = idInmueble }
        );
    }

    private void GuardarImagen(
        int inmuebleId,
        IFormFile archivo)
    {
        if (archivo == null ||
            archivo.Length == 0)
        {
            return;
        }

        const long tamanoMaximo =
            5 * 1024 * 1024;

        if (archivo.Length > tamanoMaximo)
        {
            TempData["Error"] =
                "Una de las imágenes supera el tamaño máximo permitido de 5 MB.";

            return;
        }

        var extensionesPermitidas =
            new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

        var extension =
            Path.GetExtension(
                archivo.FileName
            ).ToLowerInvariant();

        if (!extensionesPermitidas.Contains(
                extension))
        {
            TempData["Error"] =
                "Solo se permiten imágenes JPG, JPEG, PNG o WEBP.";

            return;
        }

        var tiposPermitidos =
            new[]
            {
                "image/jpeg",
                "image/png",
                "image/webp"
            };

        if (!tiposPermitidos.Contains(
                archivo.ContentType.ToLowerInvariant()))
        {
            TempData["Error"] =
                "El archivo seleccionado no es un tipo de imagen permitido.";

            return;
        }

        var carpeta =
            Path.Combine(
                environment.WebRootPath,
                "images",
                "inmuebles",
                inmuebleId.ToString()
            );

        Directory.CreateDirectory(carpeta);

        var nombreArchivo =
            $"{Guid.NewGuid()}{extension}";

        var rutaFisica =
            Path.Combine(
                carpeta,
                nombreArchivo
            );

        using var stream =
            new FileStream(
                rutaFisica,
                FileMode.Create
            );

        archivo.CopyTo(stream);

        var url =
            $"/images/inmuebles/{inmuebleId}/{nombreArchivo}";

        var tienePortada =
            repositorioImagen.TienePortada(
                inmuebleId
            );

        var imagen =
            new ImagenInmueble
            {
                InmuebleId = inmuebleId,
                Url = url,
                EsPortada = !tienePortada
            };

        repositorioImagen.Alta(
            imagen
        );
    }

    private void EliminarArchivoFisico(
        string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return;
        }

        if (!url.StartsWith(
                "/images/",
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var rutaRelativa =
            url.TrimStart('/')
                .Replace(
                    '/',
                    Path.DirectorySeparatorChar
                );

        var rutaFisica =
            Path.Combine(
                environment.WebRootPath,
                rutaRelativa
            );

        if (System.IO.File.Exists(
                rutaFisica))
        {
            System.IO.File.Delete(
                rutaFisica
            );
        }
    }
}

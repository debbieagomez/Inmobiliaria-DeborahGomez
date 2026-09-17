using Inmobiliaria_DeborahGomez.Models;
using Inmobiliaria_DeborahGomez.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("MySqlConnection");

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepositorioPropietario>(
    provider => new RepositorioPropietario(connectionString!));
builder.Services.AddScoped<IRepositorioInquilino>(
    provider => new RepositorioInquilino(connectionString!));
builder.Services.AddScoped<IRepositorioTipoInmueble>(
    provider => new RepositorioTipoInmueble(connectionString!));

builder.Services.AddScoped<IRepositorioInmueble>(
    provider => new RepositorioInmueble(connectionString!));

builder.Services.AddScoped<IRepositorioReserva>(
    provider => new RepositorioReserva(connectionString!));

builder.Services.AddScoped<IRepositorioUsuario>(
    provider => new RepositorioUsuario(connectionString!));

builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();

builder.Services.AddScoped<IRepositorioPago>(
    provider =>
        new RepositorioPago(connectionString!));


builder.Services
    .AddAuthentication(
        CookieAuthenticationDefaults.AuthenticationScheme
    )
    .AddCookie(options =>
    {
        options.LoginPath = "/Cuenta/Login";
        options.LogoutPath = "/Cuenta/Logout";
        options.AccessDeniedPath = "/Cuenta/AccesoDenegado";

        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
)
.WithStaticAssets();

app.Run();
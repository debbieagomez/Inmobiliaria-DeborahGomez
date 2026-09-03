using Inmobiliaria_DeborahGomez.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IRepositorioPropietario>(provider => new RepositorioPropietario(connectionString!));
builder.Services.AddScoped<IRepositorioInquilino>(provider => new RepositorioInquilino(connectionString!));
builder.Services.AddScoped<IRepositorioTipoInmueble>(provider => new RepositorioTipoInmueble(connectionString!));
builder.Services.AddScoped<IRepositorioReserva>( 
    provider => new RepositorioReserva(connectionString!) );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

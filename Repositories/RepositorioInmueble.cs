using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;
public class RepositorioInmueble : IRepositorioInmueble
{

    private readonly string connectionString;

    public RepositorioInmueble (string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IList<Inmueble> ObtenerLista(string? busqueda = null, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Inmueble>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Latitud, i.Longitud, i.PrecioPorDia,
                            i.PorcentajeSenia, i.Disponible, i.ImagenPortadaUrl, i.PropietarioId, i.TipoInmuebleId,
                            CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre, t.Nombre AS TipoNombre
                    FROM Inmueble i
                    JOIN Propietario p ON i.PropietarioId = p.IdPropietario
                    JOIN TipoInmueble t ON i.TipoInmuebleId = t.IdTipoInmueble
                    WHERE @busqueda IS NULL OR @busqueda = '' OR i.Direccion LIKE @busqueda
                    ORDER BY i.IdInmueble
                    LIMIT @tamPagina OFFSET @offset;";

        using var comando = new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue("@busqueda", DBNull.Value);
        }
        else
        {
            comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
        }

        var offset = (pagina - 1) * tamPagina;
        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        comando.Parameters.AddWithValue("@offset", offset);

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(LeerInmueble(reader));
        }

        return lista;
    }

    public Inmueble? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Latitud, i.Longitud, i.PrecioPorDia,
                            i.PorcentajeSenia, i.Disponible, i.ImagenPortadaUrl, i.PropietarioId, i.TipoInmuebleId,
                            CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre, t.Nombre AS TipoNombre
                    FROM Inmueble i
                    JOIN Propietario p ON i.PropietarioId = p.IdPropietario
                    JOIN TipoInmueble t ON i.TipoInmuebleId = t.IdTipoInmueble
                    WHERE i.IdInmueble = @id;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            return LeerInmueble(reader);
        }

        return null;
    }

    public int Alta(Inmueble inmueble)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"INSERT INTO Inmueble
                        (Direccion, Cupo, Latitud, Longitud, PrecioPorDia, PorcentajeSenia, Disponible, ImagenPortadaUrl, PropietarioId, TipoInmuebleId)
                    VALUES
                        (@direccion, @cupo, @latitud, @longitud, @precioPorDia, @porcentajeSenia, @disponible, @imagenPortadaUrl, @propietarioId, @tipoInmuebleId);";

        using var comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, inmueble);

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Inmueble inmueble)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"UPDATE Inmueble SET
                        Direccion = @direccion,
                        Cupo = @cupo,
                        Latitud = @latitud,
                        Longitud = @longitud,
                        PrecioPorDia = @precioPorDia,
                        PorcentajeSenia = @porcentajeSenia,
                        Disponible = @disponible,
                        ImagenPortadaUrl = @imagenPortadaUrl,
                        PropietarioId = @propietarioId,
                        TipoInmuebleId = @tipoInmuebleId
                    WHERE IdInmueble = @id;";

        using var comando = new MySqlCommand(sql, conexion);
        AgregarParametros(comando, inmueble);
        comando.Parameters.AddWithValue("@id", inmueble.IdInmueble);

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"DELETE FROM Inmueble WHERE IdInmueble = @id;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery();
    }

    public int ObtenerCantidad(string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM Inmueble i
                    WHERE @busqueda IS NULL OR @busqueda = '' OR i.Direccion LIKE @busqueda;";

        using var comando = new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue("@busqueda", DBNull.Value);
        }
        else
        {
            comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
        }

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    // ---------- Métodos privados auxiliares ----------

    private static Inmueble LeerInmueble(MySqlDataReader reader)
    {
        return new Inmueble
        {
            IdInmueble = reader.GetInt32("IdInmueble"),
            Direccion = reader.GetString("Direccion"),
            Cupo = reader.GetInt32("Cupo"),
            Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud")) ? null : reader.GetDecimal("Latitud"),
            Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud")) ? null : reader.GetDecimal("Longitud"),
            PrecioPorDia = reader.GetDecimal("PrecioPorDia"),
            PorcentajeSenia = reader.GetDecimal("PorcentajeSenia"),
            Disponible = reader.GetBoolean("Disponible"),
            ImagenPortadaUrl = reader.IsDBNull(reader.GetOrdinal("ImagenPortadaUrl")) ? null : reader.GetString("ImagenPortadaUrl"),
            PropietarioId = reader.GetInt32("PropietarioId"),
            TipoInmuebleId = reader.GetInt32("TipoInmuebleId"),
            propietarioNombre = reader.GetString("propietarioNombre"),
            TipoNombre = reader.GetString("TipoNombre")
        };
    }

    private static void AgregarParametros(MySqlCommand comando, Inmueble inmueble)
    {
        comando.Parameters.AddWithValue("@direccion", inmueble.Direccion);
        comando.Parameters.AddWithValue("@cupo", inmueble.Cupo);
        comando.Parameters.AddWithValue("@latitud", (object?)inmueble.Latitud ?? DBNull.Value);
        comando.Parameters.AddWithValue("@longitud", (object?)inmueble.Longitud ?? DBNull.Value);
        comando.Parameters.AddWithValue("@precioPorDia", inmueble.PrecioPorDia);
        comando.Parameters.AddWithValue("@porcentajeSenia", inmueble.PorcentajeSenia);
        comando.Parameters.AddWithValue("@disponible", inmueble.Disponible);
        comando.Parameters.AddWithValue("@imagenPortadaUrl", (object?)inmueble.ImagenPortadaUrl ?? DBNull.Value);
        comando.Parameters.AddWithValue("@propietarioId", inmueble.PropietarioId);
        comando.Parameters.AddWithValue("@tipoInmuebleId", inmueble.TipoInmuebleId);
    }

}
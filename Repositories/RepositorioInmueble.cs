using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;

public class RepositorioInmueble : IRepositorioInmueble
{
    private readonly string connectionString;

    public RepositorioInmueble(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IList<Inmueble> ObtenerLista(
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Inmueble>();

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT
                i.IdInmueble,
                i.Direccion,
                i.Cupo,
                i.Latitud,
                i.Longitud,
                i.PrecioPorDia,
                i.PorcentajeSenia,
                i.Disponible,
                i.PropietarioId,
                i.TipoInmuebleId,
                CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre,
                t.Nombre AS TipoNombre
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR i.Direccion LIKE @busqueda
                OR CONCAT(p.Nombre, ' ', p.Apellido) LIKE @busqueda
                OR t.Nombre LIKE @busqueda
            ORDER BY i.IdInmueble
            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue(
                "@busqueda",
                DBNull.Value
            );
        }
        else
        {
            comando.Parameters.AddWithValue(
                "@busqueda",
                "%" + busqueda + "%"
            );
        }

        var offset =
            (pagina - 1) * tamPagina;

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            offset
        );

        using var reader =
            comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(
                LeerInmueble(reader)
            );
        }

        return lista;
    }

    public Inmueble? ObtenerPorId(int id)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT
                i.IdInmueble,
                i.Direccion,
                i.Cupo,
                i.Latitud,
                i.Longitud,
                i.PrecioPorDia,
                i.PorcentajeSenia,
                i.Disponible,
                i.PropietarioId,
                i.TipoInmuebleId,
                CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre,
                t.Nombre AS TipoNombre
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE i.IdInmueble = @id;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@id",
            id
        );

        using var reader =
            comando.ExecuteReader();

        if (reader.Read())
        {
            return LeerInmueble(reader);
        }

        return null;
    }

    public int Alta(Inmueble inmueble)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            INSERT INTO Inmueble
            (
                Direccion,
                Cupo,
                Latitud,
                Longitud,
                PrecioPorDia,
                PorcentajeSenia,
                Disponible,
                PropietarioId,
                TipoInmuebleId
            )
            VALUES
            (
                @direccion,
                @cupo,
                @latitud,
                @longitud,
                @precioPorDia,
                @porcentajeSenia,
                @disponible,
                @propietarioId,
                @tipoInmuebleId
            );
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        AgregarParametros(
            comando,
            inmueble
        );

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Inmueble inmueble)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            UPDATE Inmueble
            SET
                Direccion = @direccion,
                Cupo = @cupo,
                Latitud = @latitud,
                Longitud = @longitud,
                PrecioPorDia = @precioPorDia,
                PorcentajeSenia = @porcentajeSenia,
                Disponible = @disponible,
                PropietarioId = @propietarioId,
                TipoInmuebleId = @tipoInmuebleId
            WHERE IdInmueble = @id;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        AgregarParametros(
            comando,
            inmueble
        );

        comando.Parameters.AddWithValue(
            "@id",
            inmueble.IdInmueble
        );

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            DELETE FROM Inmueble
            WHERE IdInmueble = @id;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@id",
            id
        );

        return comando.ExecuteNonQuery();
    }

    public int ObtenerCantidad(
        string? busqueda = null)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR i.Direccion LIKE @busqueda
                OR CONCAT(p.Nombre, ' ', p.Apellido) LIKE @busqueda
                OR t.Nombre LIKE @busqueda;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue(
                "@busqueda",
                DBNull.Value
            );
        }
        else
        {
            comando.Parameters.AddWithValue(
                "@busqueda",
                "%" + busqueda + "%"
            );
        }

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public IList<Inmueble> ObtenerPorDisponibilidad(
        bool? disponible,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Inmueble>();

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT
                i.IdInmueble,
                i.Direccion,
                i.Cupo,
                i.Latitud,
                i.Longitud,
                i.PrecioPorDia,
                i.PorcentajeSenia,
                i.Disponible,
                i.PropietarioId,
                i.TipoInmuebleId,
                CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre,
                t.Nombre AS TipoNombre
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE
                @disponible IS NULL
                OR i.Disponible = @disponible
            ORDER BY i.Direccion
            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        if (disponible.HasValue)
        {
            comando.Parameters.AddWithValue(
                "@disponible",
                disponible.Value
            );
        }
        else
        {
            comando.Parameters.AddWithValue(
                "@disponible",
                DBNull.Value
            );
        }

        var offset =
            (pagina - 1) * tamPagina;

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            offset
        );

        using var reader =
            comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(
                LeerInmueble(reader)
            );
        }

        return lista;
    }

    public int ObtenerCantidadPorDisponibilidad(
        bool? disponible)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Inmueble i
            WHERE
                @disponible IS NULL
                OR i.Disponible = @disponible;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        if (disponible.HasValue)
        {
            comando.Parameters.AddWithValue(
                "@disponible",
                disponible.Value
            );
        }
        else
        {
            comando.Parameters.AddWithValue(
                "@disponible",
                DBNull.Value
            );
        }

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    // Informe 2: Inmuebles de un propietario específico 

    public IList<Inmueble> ObtenerPorPropietario(int propietarioId, int pagina = 1, int tamPagina = 10)
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
                    WHERE i.PropietarioId = @propietarioId
                    ORDER BY i.Direccion
                    LIMIT @tamPagina OFFSET @offset;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@propietarioId", propietarioId);

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

    // Informe 3: Inmuebles más reservados en los últimos X días 

    public IList<InmuebleConReservas> ObtenerMasReservados(int dias = 365, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<InmuebleConReservas>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Latitud, i.Longitud, i.PrecioPorDia,
                            i.PorcentajeSenia, i.Disponible, i.PropietarioId, i.TipoInmuebleId,
                            CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre, t.Nombre AS TipoNombre,
                            COUNT(r.IdReserva) AS CantidadReservas
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.PropietarioId = p.IdPropietario
                    INNER JOIN TipoInmueble t ON i.TipoInmuebleId = t.IdTipoInmueble
                    INNER JOIN Reserva r ON r.InmuebleId = i.IdInmueble
                        AND r.FechaDesde >= DATE_SUB(NOW(), INTERVAL @dias DAY)
                    GROUP BY i.IdInmueble
                    ORDER BY CantidadReservas DESC
                    LIMIT @tamPagina OFFSET @offset;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@dias", dias);

        var offset = (pagina - 1) * tamPagina;
        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        comando.Parameters.AddWithValue("@offset", offset);

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new InmuebleConReservas
            {
                Inmueble = LeerInmueble(reader),
                CantidadReservas = reader.GetInt32("CantidadReservas")
            });
        }

        return lista;
    }

//Informe 4: Inmuebles sin reservas en los últimos X días

    public IList<Inmueble> ObtenerSinReservas(int dias, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Inmueble>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Latitud, i.Longitud, i.PrecioPorDia,
                            i.PorcentajeSenia, i.Disponible, i.PropietarioId, i.TipoInmuebleId,
                            CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre, t.Nombre AS TipoNombre
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.PropietarioId = p.IdPropietario
                    INNER JOIN TipoInmueble t ON i.TipoInmuebleId = t.IdTipoInmueble
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Reserva r
                        WHERE r.InmuebleId = i.IdInmueble
                        AND r.FechaDesde >= DATE_SUB(NOW(), INTERVAL @dias DAY)
                    )
                    ORDER BY i.Direccion
                    LIMIT @tamPagina OFFSET @offset;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@dias", dias);

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

    //  Informe 5: Inmuebles disponibles (no ocupados) entre dos fechas 

    public IList<Inmueble> ObtenerDisponiblesEntreFechas(DateTime fechaDesde, DateTime fechaHasta, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Inmueble>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        // Un inmueble está "ocupado" en el rango si tiene alguna reserva cuyas fechas se solapan
        // con el rango buscado. Buscamos los que NO tengan ninguna reserva así.
        var sql = @"SELECT i.IdInmueble, i.Direccion, i.Cupo, i.Latitud, i.Longitud, i.PrecioPorDia,
                            i.PorcentajeSenia, i.Disponible, i.PropietarioId, i.TipoInmuebleId,
                            CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre, t.Nombre AS TipoNombre
                    FROM Inmueble i
                    INNER JOIN Propietario p ON i.PropietarioId = p.IdPropietario
                    INNER JOIN TipoInmueble t ON i.TipoInmuebleId = t.IdTipoInmueble
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Reserva r
                        WHERE r.InmuebleId = i.IdInmueble
                          AND r.FechaDesde < @fechaHasta
                          AND r.FechaHasta > @fechaDesde
                    )
                    ORDER BY i.Direccion
                    LIMIT @tamPagina OFFSET @offset;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);

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

    public int ObtenerCantidadDisponiblesEntreFechas(DateTime fechaDesde, DateTime fechaHasta)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM Inmueble i
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Reserva r
                        WHERE r.InmuebleId = i.IdInmueble
                            AND r.FechaDesde < @fechaHasta
                            AND r.FechaHasta > @fechaDesde
                    );";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public int ObtenerCantidadSinReservas(int dias)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM Inmueble i
                    WHERE NOT EXISTS (
                        SELECT 1 FROM Reserva r
                        WHERE r.InmuebleId = i.IdInmueble
                            AND r.FechaDesde >= DATE_SUB(NOW(), INTERVAL @dias DAY)
                    );";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@dias", dias);

        return Convert.ToInt32(comando.ExecuteScalar());
    }
    public int ObtenerCantidadMasReservados(int dias = 365)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM (
                        SELECT i.IdInmueble
                        FROM Inmueble i
                        INNER JOIN Reserva r ON r.InmuebleId = i.IdInmueble
                            AND r.FechaDesde >= DATE_SUB(NOW(), INTERVAL @dias DAY)
                        GROUP BY i.IdInmueble
                    ) AS sub;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@dias", dias);

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public int ObtenerCantidadPorPropietario(int propietarioId)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM Inmueble i WHERE i.PropietarioId = @propietarioId;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@propietarioId", propietarioId);

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    private static Inmueble LeerInmueble(
        MySqlDataReader reader)
    {
        return new Inmueble
        {
            IdInmueble =
                reader.GetInt32(
                    "IdInmueble"
                ),

            Direccion =
                reader.GetString(
                    "Direccion"
                ),

            Cupo =
                reader.GetInt32(
                    "Cupo"
                ),

            Latitud =
                reader.IsDBNull(
                    reader.GetOrdinal(
                        "Latitud"
                    )
                )
                    ? null
                    : reader.GetDecimal(
                        "Latitud"
                    ),

            Longitud =
                reader.IsDBNull(
                    reader.GetOrdinal(
                        "Longitud"
                    )
                )
                    ? null
                    : reader.GetDecimal(
                        "Longitud"
                    ),

            PrecioPorDia =
                reader.GetDecimal(
                    "PrecioPorDia"
                ),

            PorcentajeSenia =
                reader.GetDecimal(
                    "PorcentajeSenia"
                ),

            Disponible =
                reader.GetBoolean(
                    "Disponible"
                ),

            PropietarioId =
                reader.GetInt32(
                    "PropietarioId"
                ),

            TipoInmuebleId =
                reader.GetInt32(
                    "TipoInmuebleId"
                ),

            propietarioNombre =
                reader.GetString(
                    "propietarioNombre"
                ),

            TipoNombre =
                reader.GetString(
                    "TipoNombre"
                )
        };
    }

    private static void AgregarParametros(
        MySqlCommand comando,
        Inmueble inmueble)
    {
        comando.Parameters.AddWithValue(
            "@direccion",
            inmueble.Direccion
        );

        comando.Parameters.AddWithValue(
            "@cupo",
            inmueble.Cupo
        );

        comando.Parameters.AddWithValue(
            "@latitud",
            (object?)inmueble.Latitud
                ?? DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@longitud",
            (object?)inmueble.Longitud
                ?? DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@precioPorDia",
            inmueble.PrecioPorDia
        );

        comando.Parameters.AddWithValue(
            "@porcentajeSenia",
            inmueble.PorcentajeSenia
        );

        comando.Parameters.AddWithValue(
            "@disponible",
            inmueble.Disponible
        );

        comando.Parameters.AddWithValue(
            "@propietarioId",
            inmueble.PropietarioId
        );

        comando.Parameters.AddWithValue(
            "@tipoInmuebleId",
            inmueble.TipoInmuebleId
        );
    }
}
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
                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,
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
                OR CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) LIKE @busqueda
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
                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,
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
                OR CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) LIKE @busqueda
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
                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,
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

    public IList<Inmueble> ObtenerPorPropietario(
        int propietarioId,
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
                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,
                t.Nombre AS TipoNombre
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE i.PropietarioId = @propietarioId
            ORDER BY i.Direccion
            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@propietarioId",
            propietarioId
        );

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            (pagina - 1) * tamPagina
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

    public IList<InmuebleConReservas> ObtenerMasReservados(
        int dias = 365,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista =
            new List<InmuebleConReservas>();

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var fechaCorte =
            DateTime.Now.AddDays(-dias);

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

                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,

                t.Nombre AS TipoNombre,

                r.CantidadReservas,
                r.UltimaFecha

            FROM Inmueble i

            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario

            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble

            INNER JOIN
            (
                SELECT
                    InmuebleId,
                    COUNT(*) AS CantidadReservas,
                    MAX(FechaDesde) AS UltimaFecha
                FROM Reserva
                WHERE FechaDesde >= @fechaCorte
                GROUP BY InmuebleId
            ) r
                ON r.InmuebleId = i.IdInmueble

            ORDER BY
                r.CantidadReservas DESC,
                r.UltimaFecha DESC

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaCorte",
            fechaCorte
        );

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            (pagina - 1) * tamPagina
        );

        using var reader =
            comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(
                new InmuebleConReservas
                {
                    Inmueble =
                        LeerInmueble(reader),

                    CantidadReservas =
                        Convert.ToInt32(
                            reader["CantidadReservas"]
                        ),

                    UltimaFecha =
                        reader.GetDateTime(
                            "UltimaFecha"
                        )
                }
            );
        }

        return lista;
    }

    public int ObtenerCantidadMasReservados(
        int dias = 365)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var fechaCorte =
            DateTime.Now.AddDays(-dias);

        var sql = @"
            SELECT COUNT(*)
            FROM
            (
                SELECT InmuebleId
                FROM Reserva
                WHERE FechaDesde >= @fechaCorte
                GROUP BY InmuebleId
            ) AS sub;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaCorte",
            fechaCorte
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public IList<Inmueble> ObtenerSinReservas(
        int dias,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista =
            new List<Inmueble>();

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var fechaCorte =
            DateTime.Now.AddDays(-dias);

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

                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,

                t.Nombre AS TipoNombre

            FROM Inmueble i

            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario

            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble

            WHERE NOT EXISTS
            (
                SELECT 1
                FROM Reserva r

                WHERE r.InmuebleId = i.IdInmueble
                  AND r.FechaDesde >= @fechaCorte
            )

            ORDER BY i.Direccion

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaCorte",
            fechaCorte
        );

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            (pagina - 1) * tamPagina
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

    public int ObtenerCantidadSinReservas(
        int dias)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var fechaCorte =
            DateTime.Now.AddDays(-dias);

        var sql = @"
            SELECT COUNT(*)
            FROM Inmueble i

            WHERE NOT EXISTS
            (
                SELECT 1
                FROM Reserva r

                WHERE r.InmuebleId = i.IdInmueble
                  AND r.FechaDesde >= @fechaCorte
            );
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaCorte",
            fechaCorte
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public IList<Inmueble> ObtenerDisponiblesEntreFechas(
        DateTime fechaDesde,
        DateTime fechaHasta,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista =
            new List<Inmueble>();

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

                CONCAT(
                    p.Nombre,
                    ' ',
                    p.Apellido
                ) AS propietarioNombre,

                t.Nombre AS TipoNombre

            FROM Inmueble i

            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario

            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble

            WHERE i.Disponible = 1

              AND NOT EXISTS
              (
                    SELECT 1
                    FROM Reserva r

                    WHERE r.InmuebleId = i.IdInmueble

                      AND r.FechaDesde < @fechaHasta

                      AND COALESCE(
                            r.FechaFinalizacionAnticipada,
                            r.FechaHasta
                          ) > @fechaDesde
              )

            ORDER BY i.Direccion

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaDesde",
            fechaDesde
        );

        comando.Parameters.AddWithValue(
            "@fechaHasta",
            fechaHasta
        );

        comando.Parameters.AddWithValue(
            "@tamPagina",
            tamPagina
        );

        comando.Parameters.AddWithValue(
            "@offset",
            (pagina - 1) * tamPagina
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

    public int ObtenerCantidadDisponiblesEntreFechas(
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Inmueble i

            WHERE i.Disponible = 1

              AND NOT EXISTS
              (
                    SELECT 1
                    FROM Reserva r

                    WHERE r.InmuebleId = i.IdInmueble

                      AND r.FechaDesde < @fechaHasta

                      AND COALESCE(
                            r.FechaFinalizacionAnticipada,
                            r.FechaHasta
                          ) > @fechaDesde
              );
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaDesde",
            fechaDesde
        );

        comando.Parameters.AddWithValue(
            "@fechaHasta",
            fechaHasta
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public int ObtenerCantidadPorPropietario(
        int propietarioId)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Inmueble i
            WHERE i.PropietarioId = @propietarioId;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@propietarioId",
            propietarioId
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public bool TieneReservas(
        int inmuebleId)
    {
        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT EXISTS(
                SELECT 1
                FROM Reserva
                WHERE InmuebleId = @inmuebleId
            );
        ";

        using var comando =
            new MySqlCommand(
                sql,
                conexion
            );

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            inmuebleId
        );

        return Convert.ToBoolean(
            comando.ExecuteScalar()
        );
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
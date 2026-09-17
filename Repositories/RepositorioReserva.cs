using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;

public class RepositorioReserva : IRepositorioReserva
{
    private readonly string connectionString;

    public RepositorioReserva(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public bool ExisteSolapamiento(
    int inmuebleId,
    DateTime fechaDesde,
    DateTime fechaHasta,
    int? idReservaExcluir = null)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Reserva
            WHERE InmuebleId = @inmuebleId
            AND FechaDesde < @fechaHasta
            AND FechaHasta > @fechaDesde
            AND (@idReservaExcluir IS NULL OR IdReserva <> @idReservaExcluir);
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@inmuebleId", inmuebleId);
        comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);

        comando.Parameters.AddWithValue(
            "@idReservaExcluir",
            idReservaExcluir ?? (object)DBNull.Value
        );

        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;
    }

    public IList<Inmueble> BuscarDisponibles(
    DateTime fechaDesde,
    DateTime fechaHasta,
    int? cupo = null,
    int? tipoInmuebleId = null,
    decimal? precioMaximo = null)
    {
        var lista = new List<Inmueble>();

        using var conexion = new MySqlConnection(connectionString);
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
                i.ImagenPortadaUrl,
                i.PropietarioId,
                i.TipoInmuebleId,
                CONCAT(p.Nombre, ' ', p.Apellido) AS propietarioNombre,
                t.Nombre AS TipoNombre
            FROM Inmueble i
            INNER JOIN Propietario p
                ON i.PropietarioId = p.IdPropietario
            INNER JOIN TipoInmueble t
                ON i.TipoInmuebleId = t.IdTipoInmueble
            WHERE i.Disponible = 1

            AND (@cupo IS NULL OR i.Cupo >= @cupo)

            AND (@tipoInmuebleId IS NULL
                OR i.TipoInmuebleId = @tipoInmuebleId)

            AND (@precioMaximo IS NULL
                OR i.PrecioPorDia <= @precioMaximo)

            AND NOT EXISTS
            (
                SELECT 1
                FROM Reserva r
                WHERE r.InmuebleId = i.IdInmueble
                    AND r.FechaDesde < @fechaHasta
                    AND r.FechaHasta > @fechaDesde
            )

            ORDER BY i.IdInmueble;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@fechaDesde", fechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", fechaHasta);

        comando.Parameters.AddWithValue(
            "@cupo",
            cupo ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@tipoInmuebleId",
            tipoInmuebleId ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@precioMaximo",
            precioMaximo ?? (object)DBNull.Value
        );

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Inmueble
            {
                IdInmueble = reader.GetInt32("IdInmueble"),
                Direccion = reader.GetString("Direccion"),
                Cupo = reader.GetInt32("Cupo"),

                Latitud = reader.IsDBNull(reader.GetOrdinal("Latitud"))
                    ? null
                    : reader.GetDecimal("Latitud"),

                Longitud = reader.IsDBNull(reader.GetOrdinal("Longitud"))
                    ? null
                    : reader.GetDecimal("Longitud"),

                PrecioPorDia = reader.GetDecimal("PrecioPorDia"),
                PorcentajeSenia = reader.GetDecimal("PorcentajeSenia"),
                Disponible = reader.GetBoolean("Disponible"),

                ImagenPortadaUrl = reader.IsDBNull(
                    reader.GetOrdinal("ImagenPortadaUrl"))
                    ? null
                    : reader.GetString("ImagenPortadaUrl"),

                PropietarioId = reader.GetInt32("PropietarioId"),
                TipoInmuebleId = reader.GetInt32("TipoInmuebleId"),

                propietarioNombre = reader.GetString("propietarioNombre"),
                TipoNombre = reader.GetString("TipoNombre")
            });
        }

        return lista;
    }

    public IList<Reserva> ObtenerLista(
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Reserva>();

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT
                r.IdReserva,
                r.FechaDesde,
                r.FechaHasta,
                r.FechaHastaOriginal,
                r.MontoPorDia,
                r.Finalizada,
                r.FechaFinalizacionAnticipada,
                r.MontoMulta,
                r.InmuebleId,
                r.InquilinoId,
                r.UsuarioCreadorId,
                r.UsuarioFinalizadorId,

                i.Direccion AS DireccionInmueble,

                q.NombreCompleto AS NombreInquilino,

                (
                    SELECT p.Importe
                    FROM Pago p
                    WHERE
                        p.ReservaId = r.IdReserva
                        AND p.Concepto = 'Seña'
                        AND p.Anulado = 0
                    ORDER BY p.IdPago DESC
                    LIMIT 1
                ) AS MontoSena

            FROM Reserva r

            INNER JOIN Inmueble i
                ON r.InmuebleId = i.IdInmueble

            INNER JOIN Inquilino q
                ON r.InquilinoId = q.IdInquilino

            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR i.Direccion LIKE CONCAT('%', @busqueda, '%')
                OR q.NombreCompleto LIKE CONCAT('%', @busqueda, '%')

            ORDER BY
                r.IdReserva DESC

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        var offset =
            (pagina - 1) * tamPagina;

        comando.Parameters.AddWithValue(
            "@busqueda",
            string.IsNullOrWhiteSpace(busqueda)
                ? DBNull.Value
                : busqueda
        );

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
            lista.Add(new Reserva
            {
                IdReserva =
                    reader.GetInt32("IdReserva"),

                FechaDesde =
                    reader.GetDateTime("FechaDesde"),

                FechaHasta =
                    reader.GetDateTime("FechaHasta"),

                FechaHastaOriginal =
                    reader.GetDateTime("FechaHastaOriginal"),

                MontoPorDia =
                    reader.GetDecimal("MontoPorDia"),

                Finalizada =
                    reader.GetBoolean("Finalizada"),

                FechaFinalizacionAnticipada =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "FechaFinalizacionAnticipada"
                        )
                    )
                        ? null
                        : reader.GetDateTime(
                            "FechaFinalizacionAnticipada"
                        ),

                MontoMulta =
                    reader.IsDBNull(
                        reader.GetOrdinal("MontoMulta")
                    )
                        ? null
                        : reader.GetDecimal(
                            "MontoMulta"
                        ),

                InmuebleId =
                    reader.GetInt32("InmuebleId"),

                InquilinoId =
                    reader.GetInt32("InquilinoId"),

                UsuarioCreadorId =
                    reader.GetInt32("UsuarioCreadorId"),

                UsuarioFinalizadorId =
                    reader.IsDBNull(
                        reader.GetOrdinal(
                            "UsuarioFinalizadorId"
                        )
                    )
                        ? null
                        : reader.GetInt32(
                            "UsuarioFinalizadorId"
                        ),

                DireccionInmueble =
                    reader.GetString(
                        "DireccionInmueble"
                    ),

                NombreInquilino =
                    reader.GetString(
                        "NombreInquilino"
                    ),

                MontoSena =
                    reader.IsDBNull(
                        reader.GetOrdinal("MontoSena")
                    )
                        ? null
                        : reader.GetDecimal(
                            "MontoSena"
                        )
            });
        }

        return lista;
    }
    public Reserva? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                IdReserva,
                FechaDesde,
                FechaHasta,
                FechaHastaOriginal,
                MontoPorDia,
                Finalizada,
                FechaFinalizacionAnticipada,
                MontoMulta,
                InmuebleId,
                InquilinoId,
                UsuarioCreadorId,
                UsuarioFinalizadorId
            FROM Reserva
            WHERE IdReserva = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        var reserva = new Reserva
        {
            IdReserva = reader.GetInt32("IdReserva"),
            FechaDesde = reader.GetDateTime("FechaDesde"),
            FechaHasta = reader.GetDateTime("FechaHasta"),
            FechaHastaOriginal = reader.GetDateTime("FechaHastaOriginal"),
            MontoPorDia = reader.GetDecimal("MontoPorDia"),
            Finalizada = reader.GetBoolean("Finalizada"),

            FechaFinalizacionAnticipada =
                reader.IsDBNull(reader.GetOrdinal("FechaFinalizacionAnticipada"))
                    ? null
                    : reader.GetDateTime("FechaFinalizacionAnticipada"),

            MontoMulta =
                reader.IsDBNull(reader.GetOrdinal("MontoMulta"))
                    ? null
                    : reader.GetDecimal("MontoMulta"),

            InmuebleId = reader.GetInt32("InmuebleId"),
            InquilinoId = reader.GetInt32("InquilinoId"),
            UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId"),

            UsuarioFinalizadorId =
                reader.IsDBNull(reader.GetOrdinal("UsuarioFinalizadorId"))
                    ? null
                    : reader.GetInt32("UsuarioFinalizadorId")
        };

        return reserva;
    }


    public int Alta(Reserva reserva)
    {
        if (reserva.FechaHastaOriginal == default)
        {
            reserva.FechaHastaOriginal = reserva.FechaHasta;
        }

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            INSERT INTO Reserva
            (
                FechaDesde,
                FechaHasta,
                FechaHastaOriginal,
                MontoPorDia,
                Finalizada,
                FechaFinalizacionAnticipada,
                MontoMulta,
                InmuebleId,
                InquilinoId,
                UsuarioCreadorId,
                UsuarioFinalizadorId
            )
            VALUES
            (
                @fechaDesde,
                @fechaHasta,
                @fechaHastaOriginal,
                @montoPorDia,
                @finalizada,
                @fechaFinalizacionAnticipada,
                @montoMulta,
                @inmuebleId,
                @inquilinoId,
                @usuarioCreadorId,
                @usuarioFinalizadorId
            );
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaDesde",
            reserva.FechaDesde
        );

        comando.Parameters.AddWithValue(
            "@fechaHasta",
            reserva.FechaHasta
        );

        comando.Parameters.AddWithValue(
            "@fechaHastaOriginal",
            reserva.FechaHastaOriginal
        );

        comando.Parameters.AddWithValue(
            "@montoPorDia",
            reserva.MontoPorDia
        );

        comando.Parameters.AddWithValue(
            "@finalizada",
            reserva.Finalizada
        );

        comando.Parameters.AddWithValue(
            "@fechaFinalizacionAnticipada",
            reserva.FechaFinalizacionAnticipada
                ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@montoMulta",
            reserva.MontoMulta
                ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            reserva.InmuebleId
        );

        comando.Parameters.AddWithValue(
            "@inquilinoId",
            reserva.InquilinoId
        );

        comando.Parameters.AddWithValue(
            "@usuarioCreadorId",
            reserva.UsuarioCreadorId
        );

        comando.Parameters.AddWithValue(
            "@usuarioFinalizadorId",
            reserva.UsuarioFinalizadorId
                ?? (object)DBNull.Value
        );

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Reserva reserva)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            UPDATE Reserva
            SET
                FechaDesde = @fechaDesde,
                FechaHasta = @fechaHasta,
                FechaHastaOriginal = @fechaHastaOriginal,
                MontoPorDia = @montoPorDia,
                Finalizada = @finalizada,
                FechaFinalizacionAnticipada = @fechaFinalizacionAnticipada,
                MontoMulta = @montoMulta,
                InmuebleId = @inmuebleId,
                InquilinoId = @inquilinoId,
                UsuarioCreadorId = @usuarioCreadorId,
                UsuarioFinalizadorId = @usuarioFinalizadorId
            WHERE IdReserva = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaDesde",
            reserva.FechaDesde
        );

        comando.Parameters.AddWithValue(
            "@fechaHasta",
            reserva.FechaHasta
        );

        comando.Parameters.AddWithValue(
            "@fechaHastaOriginal",
            reserva.FechaHastaOriginal
        );

        comando.Parameters.AddWithValue(
            "@montoPorDia",
            reserva.MontoPorDia
        );

        comando.Parameters.AddWithValue(
            "@finalizada",
            reserva.Finalizada
        );

        comando.Parameters.AddWithValue(
            "@fechaFinalizacionAnticipada",
            reserva.FechaFinalizacionAnticipada ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@montoMulta",
            reserva.MontoMulta ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            reserva.InmuebleId
        );

        comando.Parameters.AddWithValue(
            "@inquilinoId",
            reserva.InquilinoId
        );

        comando.Parameters.AddWithValue(
            "@usuarioCreadorId",
            reserva.UsuarioCreadorId
        );

        comando.Parameters.AddWithValue(
            "@usuarioFinalizadorId",
            reserva.UsuarioFinalizadorId ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@id",
            reserva.IdReserva
        );

        return comando.ExecuteNonQuery();
    }

    // Eliminar una reserva
    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            DELETE FROM Reserva
            WHERE IdReserva = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

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
            FROM Reserva r

            INNER JOIN Inmueble i
                ON r.InmuebleId = i.IdInmueble

            INNER JOIN Inquilino q
                ON r.InquilinoId = q.IdInquilino

            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR i.Direccion LIKE CONCAT('%', @busqueda, '%')
                OR q.NombreCompleto LIKE CONCAT('%', @busqueda, '%');
        ";

        using var comando =
            new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@busqueda",
            string.IsNullOrWhiteSpace(busqueda)
                ? DBNull.Value
                : busqueda
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        );
    }

    public int AltaConSena(
    Reserva reserva,
    decimal porcentajeSena)
    {
        if (porcentajeSena < 0 ||
            porcentajeSena > 100)
        {
            throw new ArgumentOutOfRangeException(
                nameof(porcentajeSena),
                "El porcentaje de seña debe estar entre 0 y 100."
            );
        }

        if (reserva.FechaHastaOriginal == default)
        {
            reserva.FechaHastaOriginal =
                reserva.FechaHasta;
        }

        using var conexion =
            new MySqlConnection(connectionString);

        conexion.Open();

        using var transaccion =
            conexion.BeginTransaction();

        try
        {

            var sqlReserva = @"
                INSERT INTO Reserva
                (
                    FechaDesde,
                    FechaHasta,
                    FechaHastaOriginal,
                    MontoPorDia,
                    Finalizada,
                    FechaFinalizacionAnticipada,
                    MontoMulta,
                    InmuebleId,
                    InquilinoId,
                    UsuarioCreadorId,
                    UsuarioFinalizadorId
                )
                VALUES
                (
                    @fechaDesde,
                    @fechaHasta,
                    @fechaHastaOriginal,
                    @montoPorDia,
                    @finalizada,
                    @fechaFinalizacionAnticipada,
                    @montoMulta,
                    @inmuebleId,
                    @inquilinoId,
                    @usuarioCreadorId,
                    @usuarioFinalizadorId
                );

                SELECT LAST_INSERT_ID();
            ";

            using var comandoReserva =
                new MySqlCommand(
                    sqlReserva,
                    conexion,
                    transaccion
                );

            comandoReserva.Parameters.AddWithValue(
                "@fechaDesde",
                reserva.FechaDesde
            );

            comandoReserva.Parameters.AddWithValue(
                "@fechaHasta",
                reserva.FechaHasta
            );

            comandoReserva.Parameters.AddWithValue(
                "@fechaHastaOriginal",
                reserva.FechaHastaOriginal
            );

            comandoReserva.Parameters.AddWithValue(
                "@montoPorDia",
                reserva.MontoPorDia
            );

            comandoReserva.Parameters.AddWithValue(
                "@finalizada",
                reserva.Finalizada
            );

            comandoReserva.Parameters.AddWithValue(
                "@fechaFinalizacionAnticipada",
                reserva.FechaFinalizacionAnticipada
                    ?? (object)DBNull.Value
            );

            comandoReserva.Parameters.AddWithValue(
                "@montoMulta",
                reserva.MontoMulta
                    ?? (object)DBNull.Value
            );

            comandoReserva.Parameters.AddWithValue(
                "@inmuebleId",
                reserva.InmuebleId
            );

            comandoReserva.Parameters.AddWithValue(
                "@inquilinoId",
                reserva.InquilinoId
            );

            comandoReserva.Parameters.AddWithValue(
                "@usuarioCreadorId",
                reserva.UsuarioCreadorId
            );

            comandoReserva.Parameters.AddWithValue(
                "@usuarioFinalizadorId",
                reserva.UsuarioFinalizadorId
                    ?? (object)DBNull.Value
            );

            var resultado =
                comandoReserva.ExecuteScalar();

            if (resultado == null)
            {
                throw new Exception(
                    "No se pudo obtener el Id de la reserva."
                );
            }

            var idReserva =
                Convert.ToInt32(resultado);


            var cantidadDias =
                (
                    reserva.FechaHasta.Date -
                    reserva.FechaDesde.Date
                ).Days;

            if (cantidadDias <= 0)
            {
                cantidadDias = 1;
            }


            var montoTotal =
                reserva.MontoPorDia *
                cantidadDias;


            var montoSena =
                Math.Round(
                    montoTotal *
                    porcentajeSena /
                    100m,
                    2,
                    MidpointRounding.AwayFromZero
                );


            var sqlPago = @"
                INSERT INTO Pago
                (
                    Concepto,
                    FechaPago,
                    Importe,
                    Anulado,
                    FechaAnulacion,
                    ReservaId,
                    UsuarioCreadorId,
                    UsuarioAnuladorId
                )
                VALUES
                (
                    @concepto,
                    @fechaPago,
                    @importe,
                    0,
                    NULL,
                    @reservaId,
                    @usuarioCreadorId,
                    NULL
                );
            ";

            using var comandoPago =
                new MySqlCommand(
                    sqlPago,
                    conexion,
                    transaccion
                );

            comandoPago.Parameters.AddWithValue(
                "@concepto",
                "Seña"
            );

            comandoPago.Parameters.AddWithValue(
                "@fechaPago",
                DateTime.Now
            );

            comandoPago.Parameters.AddWithValue(
                "@importe",
                montoSena
            );

            comandoPago.Parameters.AddWithValue(
                "@reservaId",
                idReserva
            );

            comandoPago.Parameters.AddWithValue(
                "@usuarioCreadorId",
                reserva.UsuarioCreadorId
            );

            comandoPago.ExecuteNonQuery();


            transaccion.Commit();

            reserva.IdReserva =
                idReserva;

            return 1;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

}


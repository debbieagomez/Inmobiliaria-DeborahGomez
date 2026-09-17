using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;

public class RepositorioPago : IRepositorioPago
{
    private readonly string connectionString;

    public RepositorioPago(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public int Alta(Pago pago)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
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

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@concepto",
            pago.Concepto
        );

        comando.Parameters.AddWithValue(
            "@fechaPago",
            pago.FechaPago
        );

        comando.Parameters.AddWithValue(
            "@importe",
            pago.Importe
        );

        comando.Parameters.AddWithValue(
            "@reservaId",
            pago.ReservaId
        );

        comando.Parameters.AddWithValue(
            "@usuarioCreadorId",
            pago.UsuarioCreadorId
        );

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Pago pago)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        // La narrativa permite modificar solamente el concepto.
        // Un pago anulado no puede volver a modificarse.
        var sql = @"
            UPDATE Pago
            SET
                Concepto = @concepto
            WHERE
                IdPago = @idPago
                AND Anulado = 0;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@concepto",
            pago.Concepto
        );

        comando.Parameters.AddWithValue(
            "@idPago",
            pago.IdPago
        );

        return comando.ExecuteNonQuery();
    }

    public int Anular(
        int idPago,
        int usuarioAnuladorId
    )
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            UPDATE Pago
            SET
                Anulado = 1,
                FechaAnulacion = @fechaAnulacion,
                UsuarioAnuladorId = @usuarioAnuladorId
            WHERE
                IdPago = @idPago
                AND Anulado = 0;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@fechaAnulacion",
            DateTime.Now
        );

        comando.Parameters.AddWithValue(
            "@usuarioAnuladorId",
            usuarioAnuladorId
        );

        comando.Parameters.AddWithValue(
            "@idPago",
            idPago
        );

        return comando.ExecuteNonQuery();
    }

    public IList<Pago> ObtenerPorReserva(
        int reservaId,
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Pago>();

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                p.IdPago,
                p.Concepto,
                p.FechaPago,
                p.Importe,
                p.Anulado,
                p.FechaAnulacion,
                p.ReservaId,
                p.UsuarioCreadorId,
                p.UsuarioAnuladorId,

                i.Direccion AS DireccionInmueble,
                q.NombreCompleto AS NombreInquilino

            FROM Pago p

            INNER JOIN Reserva r
                ON p.ReservaId = r.IdReserva

            INNER JOIN Inmueble i
                ON r.InmuebleId = i.IdInmueble

            INNER JOIN Inquilino q
                ON r.InquilinoId = q.IdInquilino

            WHERE
                p.ReservaId = @reservaId

            AND
            (
                @busqueda IS NULL
                OR @busqueda = ''
                OR p.Concepto LIKE CONCAT('%', @busqueda, '%')
            )

            ORDER BY
                p.FechaPago DESC,
                p.IdPago DESC

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        var offset = (pagina - 1) * tamPagina;

        comando.Parameters.AddWithValue(
            "@reservaId",
            reservaId
        );

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

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            var pago = new Pago
            {
                IdPago = reader.GetInt32("IdPago"),

                Concepto =
                    reader.GetString("Concepto"),

                FechaPago =
                    reader.GetDateTime("FechaPago"),

                Importe =
                    reader.GetDecimal("Importe"),

                Anulado =
                    reader.GetBoolean("Anulado"),

                FechaAnulacion =
                    reader.IsDBNull(
                        reader.GetOrdinal("FechaAnulacion")
                    )
                        ? null
                        : reader.GetDateTime("FechaAnulacion"),

                ReservaId =
                    reader.GetInt32("ReservaId"),

                UsuarioCreadorId =
                    reader.GetInt32("UsuarioCreadorId"),

                UsuarioAnuladorId =
                    reader.IsDBNull(
                        reader.GetOrdinal("UsuarioAnuladorId")
                    )
                        ? null
                        : reader.GetInt32("UsuarioAnuladorId"),

                DireccionInmueble =
                    reader.IsDBNull(
                        reader.GetOrdinal("DireccionInmueble")
                    )
                        ? null
                        : reader.GetString("DireccionInmueble"),

                NombreInquilino =
                    reader.IsDBNull(
                        reader.GetOrdinal("NombreInquilino")
                    )
                        ? null
                        : reader.GetString("NombreInquilino")
            };

            lista.Add(pago);
        }

        return lista;
    }

    public int ObtenerCantidadPorReserva(
        int reservaId,
        string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Pago p
            WHERE
                p.ReservaId = @reservaId
            AND
            (
                @busqueda IS NULL
                OR @busqueda = ''
                OR p.Concepto LIKE CONCAT('%', @busqueda, '%')
            );
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@reservaId",
            reservaId
        );

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

    public IList<Pago> ObtenerLista(
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Pago>();

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                IdPago,
                Concepto,
                FechaPago,
                Importe,
                Anulado,
                FechaAnulacion,
                ReservaId,
                UsuarioCreadorId,
                UsuarioAnuladorId

            FROM Pago

            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR Concepto LIKE CONCAT('%', @busqueda, '%')

            ORDER BY
                FechaPago DESC,
                IdPago DESC

            LIMIT @tamPagina
            OFFSET @offset;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        var offset = (pagina - 1) * tamPagina;

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

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new Pago
            {
                IdPago =
                    reader.GetInt32("IdPago"),

                Concepto =
                    reader.GetString("Concepto"),

                FechaPago =
                    reader.GetDateTime("FechaPago"),

                Importe =
                    reader.GetDecimal("Importe"),

                Anulado =
                    reader.GetBoolean("Anulado"),

                FechaAnulacion =
                    reader.IsDBNull(
                        reader.GetOrdinal("FechaAnulacion")
                    )
                        ? null
                        : reader.GetDateTime("FechaAnulacion"),

                ReservaId =
                    reader.GetInt32("ReservaId"),

                UsuarioCreadorId =
                    reader.GetInt32("UsuarioCreadorId"),

                UsuarioAnuladorId =
                    reader.IsDBNull(
                        reader.GetOrdinal("UsuarioAnuladorId")
                    )
                        ? null
                        : reader.GetInt32("UsuarioAnuladorId")
            });
        }

        return lista;
    }

    public int ObtenerCantidad(
        string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Pago
            WHERE
                @busqueda IS NULL
                OR @busqueda = ''
                OR Concepto LIKE CONCAT('%', @busqueda, '%');
        ";

        using var comando = new MySqlCommand(sql, conexion);

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

    public Pago? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                p.IdPago,
                p.Concepto,
                p.FechaPago,
                p.Importe,
                p.Anulado,
                p.FechaAnulacion,
                p.ReservaId,
                p.UsuarioCreadorId,
                p.UsuarioAnuladorId,

                i.Direccion AS DireccionInmueble,
                q.NombreCompleto AS NombreInquilino

            FROM Pago p

            INNER JOIN Reserva r
                ON p.ReservaId = r.IdReserva

            INNER JOIN Inmueble i
                ON r.InmuebleId = i.IdInmueble

            INNER JOIN Inquilino q
                ON r.InquilinoId = q.IdInquilino

            WHERE
                p.IdPago = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@id",
            id
        );

        using var reader = comando.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Pago
        {
            IdPago =
                reader.GetInt32("IdPago"),

            Concepto =
                reader.GetString("Concepto"),

            FechaPago =
                reader.GetDateTime("FechaPago"),

            Importe =
                reader.GetDecimal("Importe"),

            Anulado =
                reader.GetBoolean("Anulado"),

            FechaAnulacion =
                reader.IsDBNull(
                    reader.GetOrdinal("FechaAnulacion")
                )
                    ? null
                    : reader.GetDateTime("FechaAnulacion"),

            ReservaId =
                reader.GetInt32("ReservaId"),

            UsuarioCreadorId =
                reader.GetInt32("UsuarioCreadorId"),

            UsuarioAnuladorId =
                reader.IsDBNull(
                    reader.GetOrdinal("UsuarioAnuladorId")
                )
                    ? null
                    : reader.GetInt32("UsuarioAnuladorId"),

            DireccionInmueble =
                reader.IsDBNull(
                    reader.GetOrdinal("DireccionInmueble")
                )
                    ? null
                    : reader.GetString("DireccionInmueble"),

            NombreInquilino =
                reader.IsDBNull(
                    reader.GetOrdinal("NombreInquilino")
                )
                    ? null
                    : reader.GetString("NombreInquilino")
        };
    }

    public int Baja(int id)
    {


        return 0;
    }
}
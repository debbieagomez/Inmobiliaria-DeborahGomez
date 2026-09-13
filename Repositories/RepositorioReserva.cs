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


    public IList<Reserva> ObtenerLista(
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Reserva>();

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
            ORDER BY IdReserva
            LIMIT @tamPagina OFFSET @offset;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        var offset = (pagina - 1) * tamPagina;

        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        comando.Parameters.AddWithValue("@offset", offset);

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
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

            lista.Add(reserva);
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


    public int ObtenerCantidad(string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Reserva;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        return Convert.ToInt32(comando.ExecuteScalar());
    }
}
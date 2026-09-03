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

    public IList<Reserva> ObtenerLista(string? busqueda = null, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Reserva>();

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT IdReserva,
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
            FROM reserva
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
            var reserva = new Reserva();

            reserva.IdReserva = reader.GetInt32("IdReserva");
            reserva.FechaDesde = reader.GetDateTime("FechaDesde");
            reserva.FechaHasta = reader.GetDateTime("FechaHasta");
            reserva.FechaHastaOriginal = reader.GetDateTime("FechaHastaOriginal");
            reserva.MontoPorDia = reader.GetDecimal("MontoPorDia");
            reserva.Finalizada = reader.GetBoolean("Finalizada");

            reserva.FechaFinalizacionAnticipada =
                reader.IsDBNull(reader.GetOrdinal("FechaFinalizacionAnticipada"))
                    ? null
                    : reader.GetDateTime("FechaFinalizacionAnticipada");

            reserva.MontoMulta =
                reader.IsDBNull(reader.GetOrdinal("MontoMulta"))
                    ? null
                    : reader.GetDecimal("MontoMulta");

            reserva.InmuebleId = reader.GetInt32("InmuebleId");
            reserva.InquilinoId = reader.GetInt32("InquilinoId");
            reserva.UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId");

            reserva.UsuarioFinalizadorId =
                reader.IsDBNull(reader.GetOrdinal("UsuarioFinalizadorId"))
                    ? null
                    : reader.GetInt32("UsuarioFinalizadorId");

            lista.Add(reserva);
        }

        return lista;
    }

    public Reserva? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT IdReserva,
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
            FROM reserva
            WHERE IdReserva = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            var reserva = new Reserva();

            reserva.IdReserva = reader.GetInt32("IdReserva");
            reserva.FechaDesde = reader.GetDateTime("FechaDesde");
            reserva.FechaHasta = reader.GetDateTime("FechaHasta");
            reserva.FechaHastaOriginal = reader.GetDateTime("FechaHastaOriginal");
            reserva.MontoPorDia = reader.GetDecimal("MontoPorDia");
            reserva.Finalizada = reader.GetBoolean("Finalizada");

            reserva.FechaFinalizacionAnticipada =
                reader.IsDBNull(reader.GetOrdinal("FechaFinalizacionAnticipada"))
                    ? null
                    : reader.GetDateTime("FechaFinalizacionAnticipada");

            reserva.MontoMulta =
                reader.IsDBNull(reader.GetOrdinal("MontoMulta"))
                    ? null
                    : reader.GetDecimal("MontoMulta");

            reserva.InmuebleId = reader.GetInt32("InmuebleId");
            reserva.InquilinoId = reader.GetInt32("InquilinoId");
            reserva.UsuarioCreadorId = reader.GetInt32("UsuarioCreadorId");

            reserva.UsuarioFinalizadorId =
                reader.IsDBNull(reader.GetOrdinal("UsuarioFinalizadorId"))
                    ? null
                    : reader.GetInt32("UsuarioFinalizadorId");

            return reserva;
        }

        return null;
    }

    public int Alta(Reserva reserva)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            INSERT INTO reserva
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

        comando.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);
        comando.Parameters.AddWithValue("@fechaHastaOriginal", reserva.FechaHastaOriginal);
        comando.Parameters.AddWithValue("@montoPorDia", reserva.MontoPorDia);
        comando.Parameters.AddWithValue("@finalizada", reserva.Finalizada);
        comando.Parameters.AddWithValue(
            "@fechaFinalizacionAnticipada",
            reserva.FechaFinalizacionAnticipada ?? (object)DBNull.Value
        );
        comando.Parameters.AddWithValue(
            "@montoMulta",
            reserva.MontoMulta ?? (object)DBNull.Value
        );
        comando.Parameters.AddWithValue("@inmuebleId", reserva.InmuebleId);
        comando.Parameters.AddWithValue("@inquilinoId", reserva.InquilinoId);
        comando.Parameters.AddWithValue("@usuarioCreadorId", reserva.UsuarioCreadorId);
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
            UPDATE reserva
            SET FechaDesde = @fechaDesde,
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

        comando.Parameters.AddWithValue("@fechaDesde", reserva.FechaDesde);
        comando.Parameters.AddWithValue("@fechaHasta", reserva.FechaHasta);
        comando.Parameters.AddWithValue("@fechaHastaOriginal", reserva.FechaHastaOriginal);
        comando.Parameters.AddWithValue("@montoPorDia", reserva.MontoPorDia);
        comando.Parameters.AddWithValue("@finalizada", reserva.Finalizada);

        comando.Parameters.AddWithValue(
            "@fechaFinalizacionAnticipada",
            reserva.FechaFinalizacionAnticipada ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@montoMulta",
            reserva.MontoMulta ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue("@inmuebleId", reserva.InmuebleId);
        comando.Parameters.AddWithValue("@inquilinoId", reserva.InquilinoId);
        comando.Parameters.AddWithValue("@usuarioCreadorId", reserva.UsuarioCreadorId);

        comando.Parameters.AddWithValue(
            "@usuarioFinalizadorId",
            reserva.UsuarioFinalizadorId ?? (object)DBNull.Value
        );

        comando.Parameters.AddWithValue("@id", reserva.IdReserva);

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            DELETE FROM reserva
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

        var sql = @"SELECT COUNT(*) FROM reserva;";

        using var comando = new MySqlCommand(sql, conexion);

        return Convert.ToInt32(comando.ExecuteScalar());
    }
}
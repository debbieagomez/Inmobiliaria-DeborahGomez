using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;

public class RepositorioUsuario : IRepositorioUsuario
{
    private readonly string connectionString;

    public RepositorioUsuario(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Usuario? ObtenerPorEmail(string email)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT IdUsuario, Email, PasswordHash, Rol, Avatar
            FROM Usuario
            WHERE Email = @email;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@email", email);

        using var reader = comando.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        var usuario = new Usuario
        {
            IdUsuario = reader.GetInt32("IdUsuario"),
            Email = reader.GetString("Email"),
            PasswordHash = reader.GetString("PasswordHash"),
            Rol = reader.GetString("Rol"),
            Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                ? null
                : reader.GetString("Avatar")
        };

        return usuario;
    }

    public bool ExisteEmail(string email, int idExcluir = 0)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Usuario
            WHERE Email = @email
              AND IdUsuario <> @idExcluir;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@email", email);
        comando.Parameters.AddWithValue("@idExcluir", idExcluir);

        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;
    }

    public IList<Usuario> ObtenerLista(
        string? busqueda = null,
        int pagina = 1,
        int tamPagina = 10)
    {
        var lista = new List<Usuario>();

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT IdUsuario, Email, PasswordHash, Rol, Avatar
            FROM Usuario
            WHERE @busqueda IS NULL
               OR @busqueda = ''
               OR Email LIKE @busqueda
               OR Rol LIKE @busqueda
            ORDER BY IdUsuario
            LIMIT @tamPagina OFFSET @offset;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue("@busqueda", DBNull.Value);
        }
        else
        {
            comando.Parameters.AddWithValue(
                "@busqueda",
                "%" + busqueda + "%"
            );
        }

        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        comando.Parameters.AddWithValue(
            "@offset",
            (pagina - 1) * tamPagina
        );

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            var usuario = new Usuario
            {
                IdUsuario = reader.GetInt32("IdUsuario"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Rol = reader.GetString("Rol"),
                Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                    ? null
                    : reader.GetString("Avatar")
            };

            lista.Add(usuario);
        }

        return lista;
    }

    public int ObtenerCantidad(string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM Usuario
            WHERE @busqueda IS NULL
               OR @busqueda = ''
               OR Email LIKE @busqueda
               OR Rol LIKE @busqueda;
        ";

        using var comando = new MySqlCommand(sql, conexion);

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

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public Usuario? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            SELECT IdUsuario, Email, PasswordHash, Rol, Avatar
            FROM Usuario
            WHERE IdUsuario = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Usuario
        {
            IdUsuario = reader.GetInt32("IdUsuario"),
            Email = reader.GetString("Email"),
            PasswordHash = reader.GetString("PasswordHash"),
            Rol = reader.GetString("Rol"),
            Avatar = reader.IsDBNull(reader.GetOrdinal("Avatar"))
                ? null
                : reader.GetString("Avatar")
        };
    }

    public int Alta(Usuario usuario)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            INSERT INTO Usuario
                (Email, PasswordHash, Rol, Avatar)
            VALUES
                (@email, @passwordHash, @rol, @avatar);
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@email", usuario.Email);
        comando.Parameters.AddWithValue(
            "@passwordHash",
            usuario.PasswordHash
        );
        comando.Parameters.AddWithValue("@rol", usuario.Rol);
        comando.Parameters.AddWithValue(
            "@avatar",
            usuario.Avatar ?? (object)DBNull.Value
        );

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Usuario usuario)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            UPDATE Usuario
            SET Email = @email,
                PasswordHash = @passwordHash,
                Rol = @rol,
                Avatar = @avatar
            WHERE IdUsuario = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@email", usuario.Email);
        comando.Parameters.AddWithValue(
            "@passwordHash",
            usuario.PasswordHash
        );
        comando.Parameters.AddWithValue("@rol", usuario.Rol);
        comando.Parameters.AddWithValue(
            "@avatar",
            usuario.Avatar ?? (object)DBNull.Value
        );
        comando.Parameters.AddWithValue(
            "@id",
            usuario.IdUsuario
        );

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"
            DELETE FROM Usuario
            WHERE IdUsuario = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery();
    }
}
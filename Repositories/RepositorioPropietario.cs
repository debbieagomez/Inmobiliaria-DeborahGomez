using System.Data.SqlTypes;
using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;


public class RepositorioPropietario : IRepositorioPropietario
{

    private readonly string connectionString;

    public RepositorioPropietario (string connectionString)
    {
        
        this.connectionString = connectionString;
    }

    public IList<Propietario> ObtenerLista(string? busqueda = null, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Propietario>();

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email FROM propietario WHERE @busqueda IS NULL OR @busqueda = '' OR Dni LIKE @busqueda OR Nombre LIKE @busqueda ORDER BY IdPropietario LIMIT @tamPagina OFFSET @offset;";



        using var comando = new MySqlCommand(sql, conexion);

        if (string.IsNullOrWhiteSpace(busqueda))
        {
            comando.Parameters.AddWithValue("@busqueda", DBNull.Value);
        }
        else
        {
            comando.Parameters.AddWithValue("@busqueda", "%" + busqueda + "%");
        }

        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        comando.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);

        using var reader = comando.ExecuteReader();

        while(reader.Read())
        {
            var propietario = new Propietario();
            propietario.IdPropietario = reader.GetInt32("IdPropietario");
            propietario.Nombre = reader.GetString("Nombre");
            propietario.Apellido = reader.GetString("Apellido");
            propietario.Dni = reader.GetString("Dni");
            propietario.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");
            propietario.Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono");

            lista.Add(propietario);
        }

        return lista;

    }

    public Propietario? ObtenerPorId(int id)
    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email FROM propietariO WHERE IdPropietario = @id;";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);


        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {
            var propietario = new Propietario();
            propietario.IdPropietario = reader.GetInt32("IdPropietario");
            propietario.Nombre = reader.GetString("Nombre");
            propietario.Apellido = reader.GetString("Apellido");
            propietario.Dni = reader.GetString("Dni");
            propietario.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");
            propietario.Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono");

            return propietario;
        }

        return null;
    }

    public int Alta(Propietario propietario)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"INSERT INTO propietario(Nombre, Apellido, Dni, Telefono, Email)
            VALUES (@nombre, @apellido, @dni, @telefono, @email);
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@nombre",propietario.Nombre);
        comando.Parameters.AddWithValue("@apellido",propietario.Apellido);
        comando.Parameters.AddWithValue("@dni",propietario.Dni);
        comando.Parameters.AddWithValue("@telefono",propietario.Telefono ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("@email",propietario.Email ?? (object)DBNull.Value);

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Propietario propietario)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"UPDATE propietario
            SET Nombre = @nombre, Apellido = @apellido, Dni = @dni, Telefono = @telefono, Email = @email
            WHERE IdPropietario = @id;";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@nombre",propietario.Nombre);
        comando.Parameters.AddWithValue("@apellido",propietario.Apellido);
        comando.Parameters.AddWithValue("@dni",propietario.Dni);
        comando.Parameters.AddWithValue("@telefono",propietario.Telefono ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("@email",propietario.Email ?? (object)DBNull.Value);

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"DELETE FROM propietario WHERE IdPropietario = @id;";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        return comando.ExecuteNonQuery();
    }

    public int ObtenerCantidad(string? busqueda = null)
    {
        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM propietario WHERE @busqueda IS NULL
                OR @busqueda = '' OR Nombre LIKE @busqueda OR Apellido LIKE @busqueda OR Dni LIKE @busqueda;";

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

    public bool ExisteDni(string dni, int idExcluir = 0)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM propietario WHERE Dni = @dni AND IdPropietario <> @idExcluir;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@dni", dni);

        comando.Parameters.AddWithValue("@idExcluir", idExcluir);

        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;
    }


    public bool ExisteEmail(string email, int idExcluir = 0)
    {
        using var conexion =  new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM propietario WHERE Email = @email AND IdPropietario <> @idExcluir;";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@email",email);

        comando.Parameters.AddWithValue("@idExcluir", idExcluir);
        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;
    }
}
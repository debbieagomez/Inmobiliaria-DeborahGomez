using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;



public class RepositorioInquilino : IRepositorioInquilino

{

    private readonly string connectionString;

    public RepositorioInquilino (string connectionString)

    {

        this.connectionString = connectionString;
    }
    public IList<Inquilino> ObtenerLista(string? busqueda = null, int pagina = 1, int tamPagina = 10)
    {

        var lista = new List<Inquilino>();

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT IdInquilino, Dni, NombreCompleto, Telefono, Email FROM inquilino WHERE @busqueda IS NULL OR @busqueda = '' OR Dni LIKE @busqueda OR NombreCompleto LIKE @busqueda ORDER BY IdInquilino LIMIT @tamPagina OFFSET @offset;";

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

        while(reader.Read())
        {

            var inquilino = new Inquilino();
            inquilino.IdInquilino = reader.GetInt32("IdInquilino");
            inquilino.Dni = reader.GetString("Dni");
            inquilino.NombreCompleto = reader.GetString("NombreCompleto");
            inquilino.Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono");
            inquilino.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");
            lista.Add(inquilino);

        }
        return lista;

    }

    public Inquilino? ObtenerPorId(int id)

    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT IdInquilino, Dni, NombreCompleto, Telefono, Email FROM inquilino WHERE IdInquilino = @id;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        using var reader = comando.ExecuteReader();

        if (reader.Read())

        {

            var inquilino = new Inquilino();
            inquilino.IdInquilino = reader.GetInt32("IdInquilino");
            inquilino.Dni = reader.GetString("Dni");
            inquilino.NombreCompleto = reader.GetString("NombreCompleto");
            inquilino.Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString("Telefono");
            inquilino.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString("Email");
            return inquilino;

        }

        return null;

    }

    public int Alta(Inquilino inquilino)

    {

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();
        var sql = @"INSERT INTO inquilino(Dni, NombreCompleto, Telefono, Email) VALUES (@dni, @nombreCompleto, @telefono, @email);";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@dni", inquilino.Dni);
        comando.Parameters.AddWithValue("@nombreCompleto", inquilino.NombreCompleto);
        comando.Parameters.AddWithValue("@telefono", inquilino.Telefono ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("@email", inquilino.Email ?? (object)DBNull.Value);

        return comando.ExecuteNonQuery();

    }

    public int Modificacion(Inquilino inquilino)

    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();
        var sql = @"UPDATE inquilino SET Dni = @dni, NombreCompleto = @nombreCompleto, Telefono = @telefono, Email = @email WHERE IdInquilino = @id;";
        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", inquilino.IdInquilino);
        comando.Parameters.AddWithValue("@dni", inquilino.Dni);
        comando.Parameters.AddWithValue("@nombreCompleto", inquilino.NombreCompleto);
        comando.Parameters.AddWithValue("@telefono", inquilino.Telefono ?? (object)DBNull.Value);
        comando.Parameters.AddWithValue("@email", inquilino.Email ?? (object)DBNull.Value);
        return comando.ExecuteNonQuery();

    }

    public int Baja(int id)

    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"DELETE FROM inquilino WHERE IdInquilino = @id;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        return comando.ExecuteNonQuery();

    }

    public int ObtenerCantidad(string? busqueda = null)

    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM inquilino WHERE @busqueda IS NULL OR @busqueda = '' OR Dni LIKE @busqueda OR NombreCompleto LIKE @busqueda;";
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
        var sql = @"SELECT COUNT(*) FROM inquilino WHERE Dni = @dni AND IdInquilino <> @idExcluir;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@dni", dni);
        comando.Parameters.AddWithValue("@idExcluir", idExcluir);
        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;

    }


    public bool ExisteEmail(string email, int idExcluir = 0)

    {

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();
        var sql = @"SELECT COUNT(*) FROM inquilino WHERE Email = @email AND IdInquilino <> @idExcluir;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@email", email);
        comando.Parameters.AddWithValue("@idExcluir", idExcluir);
        var cantidad = Convert.ToInt32(comando.ExecuteScalar());

        return cantidad > 0;

    }

}


using Data;
using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;


namespace Inmobiliaria_DeborahGomez.Repositories;




public class RepositorioPropietario : IRepositorioPropietario
{      
  private readonly MySqlConnectionFactory _factory;
    public RepositorioPropietario(MySqlConnectionFactory factory)
    {
        _factory = factory;
    }

    public int Alta(Propietario entidad)
    {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            @"INSERT INTO propietarios (nombre, apellido, dni, telefono, email)
            VALUES (@nombre, @apellido, @dni, @telefono, @email);
            SELECT LAST_iNSERT_ID();",
            conexion);

            comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
            comando.Parameters.AddWithValue("@apellido", entidad.Apellido);
            comando.Parameters.AddWithValue("@dni", entidad.Dni);
            comando.Parameters.AddWithValue("@telefono", (object?)entidad.Telefono ?? DBNull.Value);
            comando.Parameters.AddWithValue("@email", (object?)entidad.Email ?? DBNull.Value);

            var nuevoId = Convert.ToInt32(comando.ExecuteScalar());
            return nuevoId;

        
    }

    public int Baja(int Id)
    {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            "DELETE FROM propietarios WHERE id_propietario = @id",
            conexion);
        comando.Parameters.AddWithValue("@id", Id);

        return comando.ExecuteNonQuery();
    }

    public int Modificacion(Propietario entidad)
    {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            @"UPDATE propietarios
              SET nombre = @nombre, apellido = @apellido, dni = @dni,
                  telefono = @telefono, email = @email
              WHERE id_propietario = @id",
            conexion);

        comando.Parameters.AddWithValue("@nombre", entidad.Nombre);
        comando.Parameters.AddWithValue("@apellido", entidad.Apellido);
        comando.Parameters.AddWithValue("@dni", entidad.Dni);
        comando.Parameters.AddWithValue("@telefono", (object?)entidad.Telefono ?? DBNull.Value);
        comando.Parameters.AddWithValue("@email", (object?)entidad.Email ?? DBNull.Value);
        comando.Parameters.AddWithValue("@id", entidad.IdPropietario);

        return comando.ExecuteNonQuery();
    }

    public Propietario? ObtenerPorId(int id)
    {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            "SELECT id_propietario, nombre, apellido, dni, telefono, email FROM propietarios WHERE id_propietario = @id",
            conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();
        if (reader.Read())
        {
            return MapearPropietario(reader);
        }
        return null;
    }

     public IList<Propietario> ObtenerLista(string? busqueda = null, int pagina = 1, int tamPagina = 10)
    {
        var lista = new List<Propietario>();
        using var conexion = _factory.CrearConexionAbierta();

        // Si viene texto de búsqueda, filtramos por nombre, apellido o dni.
        // Si no, traemos todos (WHERE 1=1 es un truco para no tener que armar el SQL con "if" distintos)
        var sql = @"SELECT id_propietario, nombre, apellido, dni, telefono, email
                     FROM propietarios
                     WHERE (@busqueda IS NULL OR nombre LIKE @busquedaLike
                            OR apellido LIKE @busquedaLike OR dni LIKE @busquedaLike)
                     ORDER BY apellido, nombre
                     LIMIT @tamPagina OFFSET @offset";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@busqueda", (object?)busqueda ?? DBNull.Value);
        comando.Parameters.AddWithValue("@busquedaLike", $"%{busqueda}%");
        comando.Parameters.AddWithValue("@tamPagina", tamPagina);
        // OFFSET calcula desde qué fila arrancar: página 1 = fila 0, página 2 = fila 10, etc.
        comando.Parameters.AddWithValue("@offset", (pagina - 1) * tamPagina);

        using var reader = comando.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(MapearPropietario(reader));
        }
        return lista;
    }

    public int ObtenerCantidad(string? busqueda = null)
    {
        using var conexion = _factory.CrearConexionAbierta();
        var sql = @"SELECT COUNT(*) FROM propietarios
                     WHERE (@busqueda IS NULL OR nombre LIKE @busquedaLike
                            OR apellido LIKE @busquedaLike OR dni LIKE @busquedaLike)";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@busqueda", (object?)busqueda ?? DBNull.Value);
        comando.Parameters.AddWithValue("@busquedaLike", $"%{busqueda}%");

        return Convert.ToInt32(comando.ExecuteScalar());
    }

    public bool ExisteDni(string dni, int idExcluir = 0)
     {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            "SELECT COUNT(*) FROM propietarios WHERE dni = @dni AND id_propietario != @idExcluir",
            conexion);
        comando.Parameters.AddWithValue("@dni", dni);
        comando.Parameters.AddWithValue("@idExcluir", idExcluir);

        var cantidad = Convert.ToInt32(comando.ExecuteScalar());
        return cantidad > 0;
    }

    public bool ExisteEmail(string email, int idExcluir = 0)
    {
        using var conexion = _factory.CrearConexionAbierta();
        using var comando = new MySqlCommand(
            "SELECT COUNT(*) FROM propietarios WHERE email = @email AND id_propietario != @idExcluir",
            conexion);
        comando.Parameters.AddWithValue("@email", email);
        comando.Parameters.AddWithValue("@idExcluir", idExcluir);

        var cantidad = Convert.ToInt32(comando.ExecuteScalar());
        return cantidad > 0;
    }

     // Método auxiliar privado: convierte una fila del reader en un objeto Propietario.
    // Se reutiliza en ObtenerPorId y ObtenerLista para no repetir el mismo código.
    private static Propietario MapearPropietario(MySqlDataReader reader)
    {
        return new Propietario
        {
            IdPropietario = reader.GetInt32("id_propietario"),
            Nombre = reader.GetString("nombre"),
            Apellido = reader.GetString("apellido"),
            Dni = reader.GetString("dni"),
            Telefono = reader.IsDBNull(reader.GetOrdinal("telefono")) ? null : reader.GetString("telefono"),
            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? null : reader.GetString("email"),
        };
    }
}










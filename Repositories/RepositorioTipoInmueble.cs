using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;
public class RepositorioTipoInmueble : IRepositorioTipoInmueble
{

    private readonly string connectionString;

    public RepositorioTipoInmueble (string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IList<TipoInmueble> ObtenerLista(string? busqueda = null, int pagina= 1, int tamPagina= 10)
    {
        var lista = new List<TipoInmueble>();
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT IdTipoInmueble, Nombre FROM tipoInmueble WHERE @busqueda IS NULL OR @busqueda = '' OR Nombre LIKE @busqueda ORDER BY IdTipoInmueble LIMIT @tamPagina OFFSET @offset;";
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

            var tipoInmueble = new TipoInmueble();
            tipoInmueble.IdTipoInmueble = reader.GetInt32("IdTipoInmueble");
            tipoInmueble.Nombre = reader.GetString("Nombre");
            lista.Add(tipoInmueble);

        }

        return lista;

    }

    public TipoInmueble? ObtenerPorId(int id)
    {

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();
        var sql = @"SELECT IdTipoInmueble, Nombre FROM tipoInmueble WHERE IdTipoInmueble = @id;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (reader.Read())
        {

            var tipoInmueble = new TipoInmueble();
            tipoInmueble.IdTipoInmueble = reader.GetInt32("IdTipoInmueble");
            tipoInmueble.Nombre = reader.GetString("Nombre");
            return tipoInmueble;

        }

        return null;

    }

    public int Alta(TipoInmueble tipoInmueble)
    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();
        var sql = @"INSERT INTO tipoInmueble(Nombre) VALUES (@nombre);";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@nombre", tipoInmueble.Nombre);
        return comando.ExecuteNonQuery();

    }

    public int Modificacion(TipoInmueble tipoInmueble)
    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"UPDATE tipoInmueble SET Nombre = @nombre WHERE IdTipoInmueble = @id;";

        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@nombre", tipoInmueble.Nombre);
        comando.Parameters.AddWithValue("@id", tipoInmueble.IdTipoInmueble);

        return comando.ExecuteNonQuery();

    }

    public int Baja(int id)
    {

        using var conexion = new MySqlConnection(connectionString);

        conexion.Open();

        var sql = @"DELETE FROM tipoInmueble WHERE IdTipoInmueble = @id;";
        using var comando = new MySqlCommand(sql, conexion);
        comando.Parameters.AddWithValue("@id", id);
        return comando.ExecuteNonQuery();

    }

    public int ObtenerCantidad(string? busqueda = null)
    {

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"SELECT COUNT(*) FROM tipoInmueble WHERE @busqueda IS NULL OR @busqueda = '' OR Nombre LIKE @busqueda;";
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

}
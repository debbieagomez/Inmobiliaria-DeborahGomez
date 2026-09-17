using Inmobiliaria_DeborahGomez.Models;
using MySqlConnector;

namespace Inmobiliaria_DeborahGomez.Repositories;

public class RepositorioImagenInmueble : IRepositorioImagenInmueble
{
    private readonly string connectionString;

    public RepositorioImagenInmueble(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IList<ImagenInmueble> ObtenerPorInmueble(int inmuebleId)
    {
        var lista = new List<ImagenInmueble>();

        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                IdImagenInmueble,
                InmuebleId,
                Url,
                EsPortada
            FROM ImagenInmueble
            WHERE InmuebleId = @inmuebleId
            ORDER BY EsPortada DESC, IdImagenInmueble;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            inmuebleId
        );

        using var reader = comando.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new ImagenInmueble
            {
                IdImagenInmueble =
                    reader.GetInt32("IdImagenInmueble"),

                InmuebleId =
                    reader.GetInt32("InmuebleId"),

                Url =
                    reader.GetString("Url"),

                EsPortada =
                    reader.GetBoolean("EsPortada")
            });
        }

        return lista;
    }

    public ImagenInmueble? ObtenerPorId(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT
                IdImagenInmueble,
                InmuebleId,
                Url,
                EsPortada
            FROM ImagenInmueble
            WHERE IdImagenInmueble = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        using var reader = comando.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new ImagenInmueble
        {
            IdImagenInmueble =
                reader.GetInt32("IdImagenInmueble"),

            InmuebleId =
                reader.GetInt32("InmuebleId"),

            Url =
                reader.GetString("Url"),

            EsPortada =
                reader.GetBoolean("EsPortada")
        };
    }

    public int Alta(ImagenInmueble imagen)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        if (!imagen.EsPortada && !TienePortada(imagen.InmuebleId))
        {
            imagen.EsPortada = true;
        }

        if (imagen.EsPortada)
        {
            using var quitarPortadas =
                new MySqlCommand(
                    @"
                    UPDATE ImagenInmueble
                    SET EsPortada = 0
                    WHERE InmuebleId = @inmuebleId;
                    ",
                    conexion
                );

            quitarPortadas.Parameters.AddWithValue(
                "@inmuebleId",
                imagen.InmuebleId
            );

            quitarPortadas.ExecuteNonQuery();
        }

        var sql = @"
            INSERT INTO ImagenInmueble
                (InmuebleId, Url, EsPortada)
            VALUES
                (@inmuebleId, @url, @esPortada);
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            imagen.InmuebleId
        );

        comando.Parameters.AddWithValue(
            "@url",
            imagen.Url
        );

        comando.Parameters.AddWithValue(
            "@esPortada",
            imagen.EsPortada
        );

        return comando.ExecuteNonQuery();
    }

    public int Baja(int id)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var imagen = ObtenerPorId(id);

        if (imagen == null)
        {
            return 0;
        }

        var sql = @"
            DELETE FROM ImagenInmueble
            WHERE IdImagenInmueble = @id;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue("@id", id);

        var resultado = comando.ExecuteNonQuery();

        if (imagen.EsPortada)
        {
            var siguienteSql = @"
                SELECT IdImagenInmueble
                FROM ImagenInmueble
                WHERE InmuebleId = @inmuebleId
                ORDER BY IdImagenInmueble
                LIMIT 1;
            ";

            using var siguienteComando =
                new MySqlCommand(
                    siguienteSql,
                    conexion
                );

            siguienteComando.Parameters.AddWithValue(
                "@inmuebleId",
                imagen.InmuebleId
            );

            var siguiente =
                siguienteComando.ExecuteScalar();

            if (siguiente != null)
            {
                using var portadaComando =
                    new MySqlCommand(
                        @"
                        UPDATE ImagenInmueble
                        SET EsPortada = 1
                        WHERE IdImagenInmueble = @id;
                        ",
                        conexion
                    );

                portadaComando.Parameters.AddWithValue(
                    "@id",
                    Convert.ToInt32(siguiente)
                );

                portadaComando.ExecuteNonQuery();
            }
        }

        return resultado;
    }

    public int EstablecerPortada(int inmuebleId, int imagenId)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        using var transaccion =
            conexion.BeginTransaction();

        try
        {
            using var quitarPortadas =
                new MySqlCommand(
                    @"
                    UPDATE ImagenInmueble
                    SET EsPortada = 0
                    WHERE InmuebleId = @inmuebleId;
                    ",
                    conexion,
                    transaccion
                );

            quitarPortadas.Parameters.AddWithValue(
                "@inmuebleId",
                inmuebleId
            );

            quitarPortadas.ExecuteNonQuery();

            using var establecerPortada =
                new MySqlCommand(
                    @"
                    UPDATE ImagenInmueble
                    SET EsPortada = 1
                    WHERE
                        IdImagenInmueble = @imagenId
                        AND InmuebleId = @inmuebleId;
                    ",
                    conexion,
                    transaccion
                );

            establecerPortada.Parameters.AddWithValue(
                "@imagenId",
                imagenId
            );

            establecerPortada.Parameters.AddWithValue(
                "@inmuebleId",
                inmuebleId
            );

            var resultado =
                establecerPortada.ExecuteNonQuery();

            if (resultado == 0)
            {
                transaccion.Rollback();
                return 0;
            }

            transaccion.Commit();

            return resultado;
        }
        catch
        {
            transaccion.Rollback();
            throw;
        }
    }

    public bool TienePortada(int inmuebleId)
    {
        using var conexion = new MySqlConnection(connectionString);
        conexion.Open();

        var sql = @"
            SELECT COUNT(*)
            FROM ImagenInmueble
            WHERE
                InmuebleId = @inmuebleId
                AND EsPortada = 1;
        ";

        using var comando = new MySqlCommand(sql, conexion);

        comando.Parameters.AddWithValue(
            "@inmuebleId",
            inmuebleId
        );

        return Convert.ToInt32(
            comando.ExecuteScalar()
        ) > 0;
    }
}
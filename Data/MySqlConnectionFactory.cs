
using MySqlConnector;

namespace Data;

public class MySqlConnectionFactory
{
    private readonly string _connectionString;

    public MySqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection")
         ?? throw new InvalidOperationException("no se encontro la cadena de conecion 'MySqlConnection' en appsetting.json");


    }

    public MySqlConnection CrearConexionAbierta()
    {
        var conexion = new MySqlConnection(_connectionString);
        conexion.Open();
        return conexion;
    }

    

}
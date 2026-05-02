using Npgsql;

namespace Infrastructure.Data;

public class PostgreSqlConnectionFactory
{
    private readonly string _connectionString;

    public PostgreSqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión.");
    }

    public NpgsqlConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
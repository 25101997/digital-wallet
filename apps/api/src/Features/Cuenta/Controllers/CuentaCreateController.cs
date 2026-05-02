using Features.Cuenta.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace Features.Cuenta.Controllers;

[ApiController]
[Route("api")]
public class CuentaCreateController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public CuentaCreateController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpPost("crear-cuenta")]
    public async Task<IActionResult> Create([FromBody] CuentaCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            return BadRequest(new
            {
                mensaje = "El nombre de la cuenta es obligatorio."
            });
        }

        if (string.IsNullOrWhiteSpace(dto.Tipo))
        {
            return BadRequest(new
            {
                mensaje = "El tipo de cuenta es obligatorio."
            });
        }

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.crear_cuenta(
                    @p_nombre,
                    @p_tipo,
                    @p_activa
                );
            ";

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@p_nombre", dto.Nombre.Trim());
            command.Parameters.AddWithValue("@p_tipo", dto.Tipo.Trim());
            command.Parameters.AddWithValue("@p_activa", dto.Activa);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Cuenta creada correctamente."
            });
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return BadRequest(new
            {
                mensaje = "Ya existe una cuenta con ese nombre."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al crear la cuenta.",
                detalle = ex.Message
            });
        }
    }
}
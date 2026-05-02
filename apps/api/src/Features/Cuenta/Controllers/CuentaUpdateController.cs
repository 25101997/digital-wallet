using Features.Cuenta.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Features.Cuenta.Controllers;

[ApiController]
[Route("api")]
public class CuentaUpdateController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public CuentaUpdateController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    [HttpPut("actualizar-cuenta/{idCuenta:int}")]
    public async Task<IActionResult> Update(
        int idCuenta,
        [FromBody] CuentaUpdateDTO dto
    )
    {
        if (idCuenta <= 0)
        {
            return BadRequest(new
            {
                mensaje = "El id de la cuenta no es válido."
            });
        }

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
                SELECT dw.actualizar_cuenta(
                    @p_id_cuenta,
                    @p_nombre,
                    @p_tipo,
                    @p_activa
                );
            ";

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@p_id_cuenta", idCuenta);
            command.Parameters.AddWithValue("@p_nombre", dto.Nombre.Trim());
            command.Parameters.AddWithValue("@p_tipo", dto.Tipo.Trim());
            command.Parameters.AddWithValue("@p_activa", dto.Activa);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Cuenta actualizada correctamente."
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
                mensaje = "Error interno al actualizar la cuenta.",
                detalle = ex.Message
            });
        }
    }

    [HttpPatch("activar-cuenta/{idCuenta:int}")]
    public async Task<IActionResult> Activar(int idCuenta)
    {
        if (idCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta no es válido." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.activar_cuenta(@p_id);
            ";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id", idCuenta);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Cuenta activada correctamente."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al activar la cuenta.",
                detalle = ex.Message
            });
        }
    }

    [HttpPatch("desactivar-cuenta/{idCuenta:int}")]
    public async Task<IActionResult> Desactivar(int idCuenta)
    {
        if (idCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta no es válido." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.desactivar_cuenta(@p_id);
            ";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id", idCuenta);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Cuenta desactivada correctamente."
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al desactivar la cuenta.",
                detalle = ex.Message
            });
        }
    }
}
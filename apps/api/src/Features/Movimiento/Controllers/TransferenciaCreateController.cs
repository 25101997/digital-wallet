using Features.Movimiento.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Features.Movimiento.Controllers;

[ApiController]
[Route("api/movimientos")]
public class TransferenciaCreateController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public TransferenciaCreateController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpPost("transferencia")]
    public async Task<IActionResult> Create([FromBody] TransferenciaCreateDTO dto)
    {
        if (dto.IdCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta origen no es válido." });

        if (dto.IdCuentaDestino <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta destino no es válido." });

        if (dto.IdCuenta == dto.IdCuentaDestino)
            return BadRequest(new { mensaje = "No se puede transferir a la misma cuenta." });

        if (dto.Monto <= 0)
            return BadRequest(new { mensaje = "El monto debe ser mayor a 0." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.transferir(
                    @p_id_cuenta_origen,
                    @p_id_cuenta_destino,
                    @p_monto,
                    @p_descripcion
                );
            ";

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@p_id_cuenta_origen", dto.IdCuenta);
            command.Parameters.AddWithValue("@p_id_cuenta_destino", dto.IdCuentaDestino);
            command.Parameters.AddWithValue("@p_monto", dto.Monto);
            command.Parameters.AddWithValue("@p_descripcion", (object?)dto.Descripcion?.Trim() ?? DBNull.Value);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Transferencia realizada correctamente."
            });
        }
        catch (PostgresException ex) when (ex.SqlState == "P0001")
        {
            return BadRequest(new
            {
                mensaje = ex.MessageText
            });
        }
        catch (PostgresException ex) when (ex.SqlState == "23503")
        {
            return BadRequest(new
            {
                mensaje = "La cuenta indicada no existe."
            });
        }
        catch (PostgresException ex) when (ex.SqlState == "23514")
        {
            return BadRequest(new
            {
                mensaje = "Uno de los valores no cumple con las restricciones de la tabla.",
                detalle = ex.MessageText
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al realizar la transferencia.",
                detalle = ex.Message
            });
        }
    }
}
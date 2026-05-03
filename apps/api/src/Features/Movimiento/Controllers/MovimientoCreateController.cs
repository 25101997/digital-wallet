using Features.Movimiento.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Features.Movimiento.Controllers;

[ApiController]
[Route("api/movimientos")]
public class MovimientoCreateController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public MovimientoCreateController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MovimientoCreateDTO dto)
    {
        if (dto.IdCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta no es válido." });

        if (string.IsNullOrWhiteSpace(dto.Tipo))
            return BadRequest(new { mensaje = "El tipo de movimiento es obligatorio." });

        if (dto.Tipo != "debitar" && dto.Tipo != "acreditar")
            return BadRequest(new { mensaje = "El tipo debe ser 'debitar' o 'acreditar'." });

        if (dto.Monto <= 0)
            return BadRequest(new { mensaje = "El monto debe ser mayor a 0." });

        if (string.IsNullOrWhiteSpace(dto.Via))
            return BadRequest(new { mensaje = "La vía del movimiento es obligatoria." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.crear_movimiento(
                    @p_id_cuenta,
                    @p_tipo,
                    @p_monto,
                    @p_descripcion,
                    @p_via
                );
            ";

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("@p_id_cuenta", dto.IdCuenta);
            command.Parameters.AddWithValue("@p_tipo", dto.Tipo.Trim());
            command.Parameters.AddWithValue("@p_monto", dto.Monto);
            command.Parameters.AddWithValue("@p_descripcion", (object?)dto.Descripcion ?? DBNull.Value);
            command.Parameters.AddWithValue("@p_via", dto.Via.Trim());

            var result = await command.ExecuteScalarAsync();

            var idMovimiento = Convert.ToInt32(result);

            return CreatedAtAction(
                actionName: "GetById",
                controllerName: "MovimientoRead",
                routeValues: new { idMovimiento },
                value: new
                {
                    mensaje = "Movimiento creado correctamente.",
                    idMovimiento
                }
            );
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
                mensaje = "Error interno al crear el movimiento.",
                detalle = ex.Message
            });
        }
    }
}
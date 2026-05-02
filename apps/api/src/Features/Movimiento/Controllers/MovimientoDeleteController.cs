using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Features.Movimiento.Controllers;

[ApiController]
[Route("api/movimientos")]
public class MovimientoDeleteController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public MovimientoDeleteController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpDelete("{idMovimiento:int}")]
    public async Task<IActionResult> Delete(int idMovimiento)
    {
        if (idMovimiento <= 0)
            return BadRequest(new { mensaje = "El id del movimiento no es válido." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"
                SELECT dw.eliminar_movimiento(@p_id_movimiento);
            ";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id_movimiento", idMovimiento);

            await command.ExecuteNonQueryAsync();

            return Ok(new
            {
                mensaje = "Movimiento eliminado correctamente."
            });
        }
        catch (PostgresException ex) when (ex.SqlState == "P0001")
        {
            return NotFound(new
            {
                mensaje = ex.MessageText
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al eliminar el movimiento.",
                detalle = ex.Message
            });
        }
    }
}
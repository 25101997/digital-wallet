using Features.Movimiento.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Features.Movimiento.Controllers;

[ApiController]
[Route("api/movimientos")]
public class MovimientoReadController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public MovimientoReadController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var movimientos = new List<MovimientoReadDTO>();

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"SELECT * FROM dw.consultar_movimientos();";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                movimientos.Add(new MovimientoReadDTO
                {
                    IdMovimiento = reader.GetInt32(reader.GetOrdinal("id_movimiento")),
                    IdCuenta = reader.GetInt32(reader.GetOrdinal("id_cuenta")),
                    Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                    Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("descripcion")),
                    Mes = reader.IsDBNull(reader.GetOrdinal("mes")) ? 0 : reader.GetInt32(reader.GetOrdinal("mes")),
                    Anio = reader.IsDBNull(reader.GetOrdinal("anio")) ? 0 : reader.GetInt32(reader.GetOrdinal("anio")),
                    Via = reader.GetString(reader.GetOrdinal("via")),
                    IdCuentaOrigen = reader.IsDBNull(reader.GetOrdinal("id_cuenta_origen"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("id_cuenta_origen")),
                    IdCuentaDestino = reader.IsDBNull(reader.GetOrdinal("id_cuenta_destino"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("id_cuenta_destino")),
                    Created = reader.GetDateTime(reader.GetOrdinal("created")),
                    Updated = reader.GetDateTime(reader.GetOrdinal("updated"))
                });
            }

            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al consultar los movimientos.",
                detalle = ex.Message
            });
        }
    }

    [HttpGet("{idMovimiento:int}", Name = "GetMovimientoById")]
    public async Task<IActionResult> GetById(int idMovimiento)
    {
        if (idMovimiento <= 0)
            return BadRequest(new { mensaje = "El id del movimiento no es válido." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"SELECT * FROM dw.consultar_movimientos_por_id(@p_id_movimiento);";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id_movimiento", idMovimiento);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return NotFound(new { mensaje = "El movimiento no existe." });

            var movimiento = new MovimientoReadDTO
            {
                IdMovimiento = reader.GetInt32(reader.GetOrdinal("id_movimiento")),
                IdCuenta = reader.GetInt32(reader.GetOrdinal("id_cuenta")),
                NombreCuenta = reader.IsDBNull(reader.GetOrdinal("nombre_cuenta"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("nombre_cuenta")),
                Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("descripcion")),
                Mes = reader.IsDBNull(reader.GetOrdinal("mes")) ? 0 : reader.GetInt32(reader.GetOrdinal("mes")),
                Anio = reader.IsDBNull(reader.GetOrdinal("anio")) ? 0 : reader.GetInt32(reader.GetOrdinal("anio")),
                Via = reader.GetString(reader.GetOrdinal("via")),
                IdCuentaOrigen = reader.IsDBNull(reader.GetOrdinal("id_cuenta_origen"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("id_cuenta_origen")),
                NombreCuentaOrigen = reader.IsDBNull(reader.GetOrdinal("nombre_cuenta_origen"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("nombre_cuenta_origen")),
                IdCuentaDestino = reader.IsDBNull(reader.GetOrdinal("id_cuenta_destino"))
                    ? null
                    : reader.GetInt32(reader.GetOrdinal("id_cuenta_destino")),
                NombreCuentaDestino = reader.IsDBNull(reader.GetOrdinal("nombre_cuenta_destino"))
                    ? null
                    : reader.GetString(reader.GetOrdinal("nombre_cuenta_destino")),
                Created = reader.GetDateTime(reader.GetOrdinal("created")),
                Updated = reader.GetDateTime(reader.GetOrdinal("updated"))
            };

            return Ok(movimiento);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al consultar el movimiento.",
                detalle = ex.Message
            });
        }
    }

    [HttpGet("cuenta/{idCuenta:int}")]
    public async Task<IActionResult> GetByCuenta(int idCuenta)
    {
        if (idCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta no es válido." });

        var movimientos = new List<MovimientoPorCuentaReadDTO>();

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = @"SELECT * FROM 
                                 dw.consultar_movimientos_por_cuenta(@p_id_cuenta);";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id_cuenta", idCuenta);

            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                movimientos.Add(new MovimientoPorCuentaReadDTO
                {
                    IdMovimiento = reader.GetInt32(reader.GetOrdinal("id_movimiento")),
                    NombreCuenta = reader.GetString(reader.GetOrdinal("nombre_cuenta")),
                    Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                    Monto = reader.GetDecimal(reader.GetOrdinal("monto")),
                    Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("descripcion")),
                    Mes = reader.IsDBNull(reader.GetOrdinal("mes")) ? 0 : reader.GetInt32(reader.GetOrdinal("mes")),
                    Anio = reader.IsDBNull(reader.GetOrdinal("anio")) ? 0 : reader.GetInt32(reader.GetOrdinal("anio")),
                    Via = reader.GetString(reader.GetOrdinal("via")),
                    IdCuentaOrigen = reader.IsDBNull(reader.GetOrdinal("id_cuenta_origen"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("id_cuenta_origen")),
                    NombreCuentaOrigen = reader.IsDBNull(reader.GetOrdinal("nombre_cuenta_origen"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("nombre_cuenta_origen")),
                    IdCuentaDestino = reader.IsDBNull(reader.GetOrdinal("id_cuenta_destino"))
                        ? null
                        : reader.GetInt32(reader.GetOrdinal("id_cuenta_destino")),
                    NombreCuentaDestino = reader.IsDBNull(reader.GetOrdinal("nombre_cuenta_destino"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("nombre_cuenta_destino")),
                    Created = reader.GetDateTime(reader.GetOrdinal("created")),
                    Updated = reader.GetDateTime(reader.GetOrdinal("updated"))
                });
            }

            return Ok(movimientos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al consultar los movimientos de la cuenta.",
                detalle = ex.Message
            });
        }
    }
}
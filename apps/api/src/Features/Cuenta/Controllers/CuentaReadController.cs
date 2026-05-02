using Features.Cuenta.DTOs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace Features.Cuenta.Controllers;

[ApiController]
[Route("api")]
public class CuentaReadController : ControllerBase
{
    private readonly PostgreSqlConnectionFactory _connectionFactory;

    public CuentaReadController(PostgreSqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    [HttpGet("cuentas")]
    public async Task<IActionResult> GetAll()
    {
        var cuentas = new List<CuentaReadDTO>();

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = "SELECT * FROM dw.listar_cuentas();";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                cuentas.Add(new CuentaReadDTO
                {
                    IdCuenta = reader.GetInt32(reader.GetOrdinal("id_cuenta")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                    Activa = reader.GetBoolean(reader.GetOrdinal("activa")),
                    Created = reader.GetDateTime(reader.GetOrdinal("created")),
                    Updated = reader.GetDateTime(reader.GetOrdinal("updated"))
                });
            }

            return Ok(cuentas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al obtener las cuentas.",
                detalle = ex.Message
            });
        }
    }

    [HttpGet("cuenta/{idCuenta:int}")]
    public async Task<IActionResult> GetById(int idCuenta)
    {
        if (idCuenta <= 0)
            return BadRequest(new { mensaje = "El id de la cuenta no es válido." });

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = "SELECT * FROM dw.obtener_cuenta_por_id(@p_id);";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@p_id", idCuenta);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return NotFound(new { mensaje = "La cuenta no existe." });

            var cuenta = new CuentaReadDTO
            {
                IdCuenta = reader.GetInt32(reader.GetOrdinal("id_cuenta")),
                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                Activa = reader.GetBoolean(reader.GetOrdinal("activa")),
                Created = reader.GetDateTime(reader.GetOrdinal("created")),
                Updated = reader.GetDateTime(reader.GetOrdinal("updated"))
            };

            return Ok(cuenta);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al obtener la cuenta.",
                detalle = ex.Message
            });
        }
    }

    [HttpGet("cuentas-con-saldos")]
    public async Task<IActionResult> ConsultarCuentaSaldo()
    {
        var cuentas = new List<CuentaSaldoReadDTO>();

        try
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync();

            const string sql = "SELECT * FROM dw.consultar_saldos();";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                cuentas.Add(new CuentaSaldoReadDTO
                {
                    IdCuenta = reader.GetInt32(reader.GetOrdinal("id_cuenta")),
                    Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                    Tipo = reader.GetString(reader.GetOrdinal("tipo")),
                    Activa = reader.GetBoolean(reader.GetOrdinal("activa")),
                    Created = reader.GetDateTime(reader.GetOrdinal("created")),
                    Updated = reader.GetDateTime(reader.GetOrdinal("updated")),
                    SaldoActual = reader.GetDecimal(reader.GetOrdinal("saldo_actual"))
                });
            }

            return Ok(cuentas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                mensaje = "Error interno al obtener las cuentas.",
                detalle = ex.Message
            });
        }
    }
}
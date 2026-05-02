namespace Features.Movimiento.DTOs;

public class MovimientoPorCuentaReadDTO
{
    public int IdMovimiento { get; set; }

    public string NombreCuenta { get; set; } = string.Empty;

    public string Tipo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public int Mes { get; set; }

    public int Anio { get; set; }

    public string Via { get; set; } = string.Empty;

    public int? IdCuentaOrigen { get; set; }

    public string? NombreCuentaOrigen { get; set; }

    public int? IdCuentaDestino { get; set; }

    public string? NombreCuentaDestino { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }
}
namespace Features.Movimiento.Models;

public class MovimientoModel
{
    public int IdMovimiento { get; set; }

    public int IdCuenta { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public int Mes { get; set; }

    public int Anio { get; set; }

    public string Via { get; set; } = string.Empty;

    public int? IdCuentaOrigen { get; set; }

    public int? IdCuentaDestino { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }
}
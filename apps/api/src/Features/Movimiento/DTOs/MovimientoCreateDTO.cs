namespace Features.Movimiento.DTOs;

public class MovimientoCreateDTO
{
    public int IdCuenta { get; set; }

    public string Tipo { get; set; } = string.Empty;

    public decimal Monto { get; set; }

    public string? Descripcion { get; set; }

    public string Via { get; set; } = string.Empty;
}
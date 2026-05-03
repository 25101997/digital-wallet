namespace Features.Movimiento.DTOs;

public class TransferenciaCreateDTO
{
    public int IdCuenta { get; set; }
    public int IdCuentaDestino { get; set; }
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
}
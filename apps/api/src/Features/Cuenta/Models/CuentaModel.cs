namespace Features.Cuenta.Models;

public class Cuenta
{
    public int IdCuenta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Activa { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
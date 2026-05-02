namespace Features.Cuenta.DTOs;

public class CuentaCreateDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Activa { get; set; } = true;
}

public class CuentaUpdateDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Activa { get; set; }
}

public class CuentaReadDTO
{
    public int IdCuenta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Activa { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}

public class CuentaSaldoReadDTO
{
    public int IdCuenta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Activa { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public decimal SaldoActual { get; set; }
}


using System.Text;

namespace BankAccountOOP;

internal class CuentaBancaria
{
    private static int numeroCuentaSemilla = 1234567890;
    public string Numero { get; }
    public int Edad { get; set; }
    public string Propietario { get; set; }
    public bool EsActivo { get; set; }
    public Genero Sexo { get; set; }
    public DateTime FechaApertura { get; }
    public decimal Balance
    {
        get
        {
            decimal balance = 0;
            foreach (var item in transacciones)
                balance += item.Monto;
            return balance;
        }
    }
    public decimal LimiteRetiro { get; set; }

    public CuentaBancaria(
        Persona persona,
        decimal balanceInicial
        )
    {
        Numero = numeroCuentaSemilla.ToString();
        numeroCuentaSemilla++;
        Propietario = persona.Nombre;
        Edad = persona.Edad;
        Sexo = persona.Sexo;
        FechaApertura = DateTime.Now;

        // se determina si es activo o jubilado por el genero y la edad del propietario
        EsActivo = SetEsActivo();
        
        // se determina el limite de extraccion en conecpto de adelanto de sueldo
        if (EsActivo)
            LimiteRetiro = 20_000;
        else
            LimiteRetiro = 10_000;

        EfectuarDeposito(balanceInicial, DateTime.Now, "Balance Inicial");
    }

    private void SetLimiteRetiro(DateTime fechaTransaccion)
    {
        if (fechaTransaccion >= FechaApertura.AddMonths(2))
            LimiteRetiro = 80_000;
    }

    private bool SetEsActivo()
    {
        if (Edad >= 65 && Sexo == Genero.Hombre)
            return false;
        else if (Edad >= 60 && Sexo == Genero.Mujer)
            return false;
        else
            return true;
    }

    private List<Transaccion> transacciones = new();


    public void EfectuarDeposito(decimal monto, DateTime fecha, string nota)
    {
        if (monto <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(monto), "El monto del deposito debe ser positivo");
        }
        SetLimiteRetiro(fecha);
        var deposito = new Transaccion(monto, LimiteRetiro, fecha, nota);
        transacciones.Add(deposito);
    }

    public void EfectuarRetiro(decimal monto, DateTime fecha, string note)
    {
        if (monto <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(monto), "El monto del retiro debe ser positivo");
        }
        SetLimiteRetiro(fecha);
        if (monto > Balance + LimiteRetiro)
        {
            // throw new ArgumentOutOfRangeException(nameof(monto),
            // "El monto del retiro no puede ser mayor al balance + limite de extraccion");
            Console.WriteLine($"No se puede realizar la transaccioin" +
                $" porque el monto ({monto:C})" +
                $" excede el limite de retiro ({(Balance + LimiteRetiro):C})");
            return;
        }
        var retiro = new Transaccion(-monto, LimiteRetiro, fecha, note);
        transacciones.Add(retiro);
    }

    public string GetHistorialDeCuenta()
    {
        var reporte = new StringBuilder();
        decimal balance = 0;
        reporte.AppendLine($"\n\nHISTORIAL DE CUENTA Nro {Numero}," +
            $" PROPIETARIO {Propietario}," +
            $" EDAD {Edad} AÑOS," +
            $" ACTIVO {(EsActivo ? "SI" : "NO")}");
        reporte.AppendLine($"CAJERO Nro 123. DIRECCION Av. Siempre Viva 123" +
            $"\n\n");
        reporte.AppendLine($"{"FECHA"}\t\t\t" +
            $"{"MONTO"}\t\t\t" +
            $"{"LIMITE RETIRO"}\t\t" +
            $"{"BALANCE"}\t\t\t" +
            $"{"NOTA"}");

        foreach (var item in transacciones)
        {
            balance += item.Monto;
            reporte.AppendLine($"{item.Fecha.ToShortDateString()}\t\t" +
                $"{item.Monto:C}\t\t" +
                $"{item.LimiteRetiro:C}\t\t" +
                $"{balance:C}\t\t" +
                $"{item.Nota}");
        }

        return reporte.ToString();
    }
}

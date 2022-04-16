namespace BankAccountOOP;

internal class Transaccion
{
    public decimal Monto { get; }
    public decimal LimiteRetiro { get; }
    public DateTime Fecha { get; }
    public string Nota { get; }

    public Transaccion(decimal monto, decimal limiteRetiro, DateTime fecha, string nota)
    {
        Monto = monto;
        LimiteRetiro = limiteRetiro;
        Fecha = fecha;
        Nota = nota;
    }
}

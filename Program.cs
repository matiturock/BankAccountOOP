using System.Globalization;

namespace BankAccountOOP;

/**
 * APLICACION DE CAJERO AUTOMATICO
 * 
 * Desarrolle una aplicación en C#, para un cajero automático. 
 * La aplicación permitirá crear cuentas para jubilados y personas en actividad.
 * Los usuarios del cajero podrán depositar en su cuenta y realizar extracciones de la misma.
 * 
 * Si el usuario es una persona en actividad laboral
 *      podrá retirar hasta 20000 pesos en concepto de adelanto de sueldo.
 * 
 * Si el usuario es una persona jubilada
 *      podrá retirar en concepto de adelanto solo 10000. 
 * 
 * Cada operación de ingreso o extracción deberá registrar la fecha, el cajero y el monto de la operación.
 * 
 * Los cajeros se identifican por su dirección y número de cajeros.
 * 
 * Si durante dos meses de operación un usuario tuvo un saldo positivo superior a $20.000 pesos,
 *      se le ofrecerá un crédito pre acordado de, $80.000 pesos. 
 *      Con lo cual, su nuevo límite de extracción en negativo será de $80.000 pesos.
*/

class Program
{
    public static void Main()
    {
        CultureInfo.CurrentCulture = new CultureInfo("es-AR"); // para setear el formato de divisa

        // prueba de cuenta activo
        var persona1 = new Persona("Messi", Genero.Hombre, 30);
        var cuenta = new CuentaBancaria(persona1, 30_000);
        Console.WriteLine($"La cuenta Nro {cuenta.Numero} fue creada por {cuenta.Propietario} con un monto inicial de {cuenta.Balance:C}");

        cuenta.EfectuarRetiro(100, DateTime.Now, "Pago de alquiler");
        Console.WriteLine($"Balance de la cuenta: {cuenta.Balance:C}");

        cuenta.EfectuarDeposito(100, DateTime.Now, "Un amigo me presto plata");
        Console.WriteLine($"Balance de la cuenta: { cuenta.Balance:C}");

        // prueba de cuenta invalida
        CuentaBancaria cuentaInvalida;
        try
        {
            var persona2 = new Persona("Invalid", Genero.Mujer, 50);
            cuentaInvalida = new CuentaBancaria(persona2, -500);
        }
        catch (ArgumentOutOfRangeException e)
        {
            Console.WriteLine("Se detectó una excepción al crear una cuenta con saldo negativo");
            Console.WriteLine(e.ToString());
        }

        // prueba de sobregiro
        try
        {
            cuenta.EfectuarRetiro(200_000, DateTime.Now, "Intento de sobregiro");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("Excepción detectada al intentar sobregirar");
            Console.WriteLine(e.ToString());
        }

        // prueba de retiro bonificado por tener la cuenta en $20.000 por dos meses o mas
        cuenta.EfectuarRetiro(80_000, DateTime.Now.AddMonths(3), "Retiro bonificado");

        // prueba de reporte de historial de cuenta
        Console.WriteLine(cuenta.GetHistorialDeCuenta());

        // prueba de cuenta jubilado
        var persona3 = new Persona("Marta", Genero.Mujer, 70);
        var cuenta2 = new CuentaBancaria(persona3, 35_000);

        cuenta2.EfectuarDeposito(1_000, DateTime.Now, "Gane la loteria");
        cuenta2.EfectuarRetiro(60_000, DateTime.Now.AddMonths(3), "Se viene navidad");

        Console.WriteLine(cuenta2.GetHistorialDeCuenta());
    }
}
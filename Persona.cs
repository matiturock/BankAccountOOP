namespace BankAccountOOP;

internal class Persona
{
    public string Nombre { get; set; }
    public Genero Sexo { get; set; }
    public int Edad { get; set; }
    
    public Persona(string nombre, Genero sexo, int edad)
    {
        Nombre = nombre;
        Sexo = sexo;
        Edad = edad;
    }
}

enum Genero
{
    Hombre,
    Mujer
}

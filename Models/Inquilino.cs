namespace Models;

public class Inquilino
{
    
    public int IdInquilino { get; set; }

    public string Dni {get; set;} = string.Empty;

    public string NombreCompleto {get; set;} = string.Empty;

    public string ?Telefono {get; set;}

    public string ?Email {get; set; }


    
}
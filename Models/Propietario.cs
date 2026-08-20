

namespace Models; 
/* esta clase vive dentro de models
*/

public class Propietario
{
    public int IdPropietario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Dni { get; set; } = string.Empty; //le da un valor inicial para que nunca sea null por accidente
    public string? Telefono { get; set; }
    public string? Email { get; set;} //indica que este campo puede quedar vacio, a diferencia de los que son obligatorios. 
}
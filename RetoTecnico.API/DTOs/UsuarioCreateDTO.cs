using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.API.DTOs;

public class UsuarioCreateDto
{
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [StringLength(200, ErrorMessage = "El nombre no puede exceder los 200 caracteres.")]
    public required string Nombre { get; set; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
                       ErrorMessage = "La contraseña debe contener al menos una mayúscula, una minúscula, un número y un carácter especial.")]
    public required string Password { get; set; }

    public int? Idperfil { get; set; }

    public int? Estatus { get; set; }

    [Required(ErrorMessage = "El nombre de usuario para inicio de sesión es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder los 100 caracteres.")]
    public required string NombreUsuario { get; set; }
}
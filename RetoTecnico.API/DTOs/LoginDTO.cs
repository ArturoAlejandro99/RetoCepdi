using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.API.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "El nombre de usuario es requerido.")]
    public string? NombreUsuario { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida.")]
    public string? Password { get; set; }
}
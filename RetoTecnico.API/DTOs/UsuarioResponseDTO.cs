namespace RetoTecnico.API.DTOs;

public class UsuarioResponseDto
{
    public int Idusuario { get; set; }
    public string? Nombre { get; set; }
    public DateTime? Fechacreacion { get; set; }
    public string? NombreUsuario { get; set; }
    public int? Idperfil { get; set; }
    public int? Estatus { get; set; }
}
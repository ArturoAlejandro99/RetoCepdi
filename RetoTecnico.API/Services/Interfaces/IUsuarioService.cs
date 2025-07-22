using RetoTecnico.API.DTOs;
using RetoTecnico.API.Models; 
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RetoTecnico.API.Services.Interfaces;
public interface IUsuarioService
{
    Task<IEnumerable<UsuarioResponseDto>> GetAllUsuariosAsync(UsuarioQueryParamsDTO queryParams);
    Task<UsuarioResponseDto?> GetUsuarioByIdAsync(int id);
    Task<UsuarioResponseDto> CreateUsuarioAsync(UsuarioCreateDto usuarioDto);
    Task<bool> UpdateUsuarioAsync(int id, UsuarioCreateDto usuarioDto); 
    Task<bool> DeleteUsuarioAsync(int id); 
    
}
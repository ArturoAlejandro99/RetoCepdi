using RetoTecnico.API.DTOs;
using System.Threading.Tasks;

namespace RetoTecnico.API.Services.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDTO loginDto);
}